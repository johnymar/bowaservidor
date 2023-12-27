using ITA_CHILE.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ITA_CHILE.Security.Firma
{
    public static class Firma
    {
        public enum TipoXML : int
        {
            NotSet = -1,
            Envio = 0,
            DTE = 1,
            LCV = 2,
            Resultado = 3,
            RCOF = 4,
            LibroBoletas = 5,
            EnvioBoleta = 6,
            LibroGuias = 7
        }

        /// <summary>
        /// Obtiene el certificado con el cual se firmarán los documentos.
        /// El nombre del certificado debe estar configurado en: 
        /// </summary>
        /// <returns>Certificado con el cual se van a firmar los documentos.</returns>
        private static X509Certificate2 ObtenerCertificado(string nombreCertificado, string password = "")
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            string[] x1 = null;
            string[] x2 = null;
            x1 = nombreCertificado.Split(' ');
            int igual = 0;
            X509Certificate2Collection certCollection = store.Certificates;
            X509Certificate2 cert = null;
            foreach (X509Certificate2 c in certCollection)
            {
              
                x2 = c.FriendlyName.ToString().Split(' ');
                for (int y = 0; y < x1.Length - 1 && y < x2.Length - 1; y++)
                {
                    string t = x2[y];
                    string t1 = x1[y];

                    if (x1[y].Contains(x2[y]))
                    {
                        igual = igual+1;
                    }
                    else
                    {
                        igual = 0;
                    }
                }
                if (igual >= x1.Length-1)
                {
                    cert = c;
                    return cert;
                }
            }

            X509Store store2 = new X509Store(StoreLocation.LocalMachine);
            store2.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection2 = store2.Certificates;

            
            x1 = nombreCertificado.Split(' ');
            igual = 0;
            foreach (X509Certificate2 c in certCollection2)
            {
                x2 = c.FriendlyName.ToString().Split(' ');
                for(int y=0; y < x1.Length - 1 && y < x2.Length - 1; y++)
                {
                    string t = x2[y];
                    string t1 = x1[y];
                    if (x1[y].Contains(x2[y]))
                    {
                        igual = igual+1;
                    }
                    else {
                        igual = 0;
                    }
                }
                if (igual>=x1.Length-1)
                {
                    cert = c;
                    return cert;
                }
            }

            /*Intenta obtener el certificado desde un archivo y password*/
            if (cert == null && !string.IsNullOrEmpty(password))
            {
                X509Certificate2Collection certCollection3 = new X509Certificate2Collection();
                certCollection3.Import(nombreCertificado, password, X509KeyStorageFlags.PersistKeySet);
                cert = certCollection3[0];
            }

            return cert;
        }

        /// <summary>
        /// Firma el XML que se enviará al servicio GetToken del SII.
        /// </summary>
        /// <param name="seed">Valor de la semilla, obtenida de la función ParseSeed.</param>
        /// <returns>String XML que representa la semilla firmada.</returns>
        public static string firmarDocumentoSemilla(string seed, string nombreCertificado, string password = "")
        {
            X509Certificate2 certificado = ObtenerCertificado(nombreCertificado, password);
            ////
            //// Cree un nuevo documento xml y defina sus caracteristicas
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.LoadXml(seed);

            ////
            //// Cree el objeto XMLSignature.
            SignedXml signedXml = new SignedXml(doc);

            ////
            //// Agregue la clave privada al objeto xmlSignature.
            signedXml.SigningKey = certificado.PrivateKey;

            ////
            //// Obtenga el objeto signature desde el objeto SignedXml.
            Signature XMLSignature = signedXml.Signature;

            ////
            //// Cree una referencia al documento que va a firmarse
            //// si la referencia es "" se firmara todo el documento
            Reference reference = new Reference("");

            ////
            //// Representa la transformación de firma con doble cifrado para una firma XML  digital que define W3C.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            ////
            //// Agregue el objeto referenciado al obeto firma.
            XMLSignature.SignedInfo.AddReference(reference);

            ////
            //// Agregue RSAKeyValue KeyInfo  ( requerido para el SII ).
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)certificado.PrivateKey));

            ////
            //// Agregar información del certificado x509
            keyInfo.AddClause(new KeyInfoX509Data(certificado));

            //// 
            //// Agregar KeyInfo al objeto Signature 
            XMLSignature.KeyInfo = keyInfo;

            ////
            //// Cree la firma
            signedXml.ComputeSignature();

            ////
            //// Recupere la representacion xml de la firma
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            ////
            //// Agregue la representacion xml de la firma al documento xml
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            ////
            //// Limpie el documento xml de la declaracion xml ( Opcional, pera para nuestro proceso es valido  )
            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }

            ////
            //// Regrese el valor de retorno
            return doc.InnerXml;
        }

        /// <summary>
        /// Firma digitalmente un documento, con un certificado digital, dada la referencia entregada por parámetro.
        /// </summary>
        /// <param name="filePath">Ruta al archivo que se desea firmar.</param>
        /// <param name="referenceID">Referencia al elemento xml raiz que se desea firmar.</param>
        /// <returns>String XML que representa el archivo firmado digitalmente.</returns>
        public static string FirmarDocumentoPath(string filePath, string referenceID, string nombreCertificado, string password = "")
        {
            var certificado = ObtenerCertificado(nombreCertificado, password);
            

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);


            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = certificado.PrivateKey;
            Signature XMLSignature = signedXml.Signature;
            Reference reference = new Reference();
            reference.Uri = "#" + referenceID;

            XmlDsigC14NTransform t = new XmlDsigC14NTransform();
            reference.AddTransform(t);            

            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
            doc.Save(filePath);

            return doc.InnerXml;
        }

        public static string FirmarDocumentoContent(string pathResult, string referenceID, string nombreCertificado, string content, string password = "")
        {
            var certificado = ObtenerCertificado(nombreCertificado, password);

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(content);


            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = certificado.PrivateKey;
            Signature XMLSignature = signedXml.Signature;
            Reference reference = new Reference();
            reference.Uri = "#" + referenceID;

            XmlDsigC14NTransform t = new XmlDsigC14NTransform();
            reference.AddTransform(t);

            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            //using (EscapeQuotesXmlWriter exw = new EscapeQuotesXmlWriter(XmlWriter.Create(pathResult)))
            //{
            //    doc.Save(exw);
            //}

            doc.Save(pathResult);

            return doc.InnerXml;
        }


        public static string FirmarDocumentoLibro(string filePath, string referenceID, string nombreCertificado, string password = "")
        {
            var certificado = ObtenerCertificado(nombreCertificado, password);

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);
            
            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = certificado.PrivateKey;
            Signature XMLSignature = signedXml.Signature;
            Reference reference = new Reference();
            reference.Uri = "#" + referenceID;

            XmlDsigC14NTransform t = new XmlDsigC14NTransform();
            reference.AddTransform(t);


            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();


            XmlElement xmlDigitalSignature = signedXml.GetXml();
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
            
            doc.Save(filePath);

            return doc.InnerXml;
        }
        /// <summary>
        /// Ruta al módulo de una llave RSA de un documento con estructura XML LCV.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_MODULUS_RESULT = "sii:Resultado/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        /// <summary>
        /// Ruta al exponente de una llave RSA de un documento con estructura XML LCV.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_EXPONENT_RESULT = "sii:Resultado/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        /// <summary>
        /// Ruta al módulo de una llave RSA de un documento con estructura XML LCV.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_MODULUS_LCV = "sii:LibroCompraVenta/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        /// <summary>
        /// Ruta al exponente de una llave RSA de un documento con estructura XML LCV.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_EXPONENT_LCV = "sii:LibroCompraVenta/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        /// <summary>
        /// Ruta al módulo de una llave RSA de un documento con estructura XML DTE.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_MODULUS = "sii:DTE/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        /// <summary>
        /// Ruta al exponente de una llave RSA de un documento con estructura XML DTE.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_EXPONENT = "sii:DTE/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        /// <summary>
        /// Ruta al módulo de una llave RSA de un documento con estructura XML EnvioDTE.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_MODULUS_ENVIO = "sii:EnvioDTE/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        /// <summary>
        /// Ruta al exponente de una llave RSA de un documento con estructura XML EnvioDTE.
        /// Se usa para verificar la firma válida del documento.
        /// </summary>
        private const string XPATH_EXPONENT_ENVIO = "sii:EnvioDTE/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        private const string XPATH_MODULUS_RCOF = "sii:ConsumoFolios/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        private const string XPATH_EXPONENT_RCOF = "sii:ConsumoFolios/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        private const string XPATH_MODULUS_LIBROBOLETAS = "sii:LibroBoleta/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        private const string XPATH_EXPONENT_LIBROBOLETAS = "sii:LibroBoleta/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        private const string XPATH_MODULUS_ENVIO_BOLETA = "sii:EnvioBOLETA/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Modulus";

        private const string XPATH_EXPONENT_ENVIO_BOLETA = "sii:EnvioBOLETA/sig:Signature/sig:KeyInfo/sig:KeyValue/sig:RSAKeyValue/sig:Exponent";

        /// <summary>
        /// Verifica la firma de un documento.
        /// </summary>
        /// <param name="filePath">Ruta del archivo, del cual, se desea verificar su firma.</param>
        /// <returns>Retorna verdader si la firma es válida, falso de otra manera.</returns>        
        public static bool VerificarFirma(string filePath, TipoXML tipoXml, out string messageOut)
        //public static bool VerificarFirma(string filePath, bool IsEnvio = true, bool IsDTE = false, bool isLCV = false, bool isResult = false)
        {
            messageOut = string.Empty;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(System.IO.File.ReadAllText(filePath, System.Text.Encoding.GetEncoding("ISO-8859-1")));

                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("sii", string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? "http://www.sii.cl/SiiDte" : doc.DocumentElement.NamespaceURI);
                if(tipoXml == TipoXML.LibroBoletas)
                    ns.AddNamespace("sig", "http://www.sii.cl/SiiDte");
                else ns.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

                //ns.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
                string mod = string.Empty;
                string exp = string.Empty;

                switch (tipoXml)
                {
                    case TipoXML.Envio:
                        mod = doc.SelectSingleNode(XPATH_MODULUS_ENVIO, ns).InnerText;
                        exp = doc.SelectSingleNode(XPATH_EXPONENT_ENVIO, ns).InnerText;
                        break;
                    case TipoXML.EnvioBoleta:
                        mod = doc.SelectSingleNode(XPATH_MODULUS_ENVIO_BOLETA, ns).InnerText;
                        exp = doc.SelectSingleNode(XPATH_EXPONENT_ENVIO_BOLETA, ns).InnerText;
                        break;
                    case TipoXML.DTE:
                       mod = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_MODULUS.Replace("sii:", "") : XPATH_MODULUS, ns).InnerText;
                        exp = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_EXPONENT.Replace("sii:", "") : XPATH_EXPONENT, ns).InnerText;
                        break;                    
                    case TipoXML.LCV:
                        mod = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_MODULUS_LCV.Replace("sii:", "") : XPATH_MODULUS_LCV, ns).InnerText;
                        exp = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_EXPONENT_LCV.Replace("sii:", "") : XPATH_EXPONENT_LCV, ns).InnerText;
                        break;
                    case TipoXML.Resultado:
                        mod = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_MODULUS_RESULT.Replace("sii:", "") : XPATH_MODULUS_RESULT, ns).InnerText;
                        exp = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_EXPONENT_RESULT.Replace("sii:", "") : XPATH_EXPONENT_RESULT, ns).InnerText;
                        break;
                    case TipoXML.RCOF:
                        mod = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_MODULUS_RCOF.Replace("sii:", "") : XPATH_MODULUS_RCOF, ns).InnerText;
                        exp = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_EXPONENT_RCOF.Replace("sii:", "") : XPATH_EXPONENT_RCOF, ns).InnerText;
                        break;
                    case TipoXML.LibroBoletas:
                        mod = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_MODULUS_LIBROBOLETAS.Replace("sii:", "") : XPATH_MODULUS_LIBROBOLETAS, ns).InnerText;
                        exp = doc.SelectSingleNode(string.IsNullOrEmpty(doc.DocumentElement.NamespaceURI) ? XPATH_EXPONENT_LIBROBOLETAS.Replace("sii:", "") : XPATH_EXPONENT_LIBROBOLETAS, ns).InnerText;
                        break;
                    default:
                        throw new Exception();
                }

                string publicKeyXml = string.Empty;

                publicKeyXml += "<RSAKeyValue>";
                publicKeyXml += "<Modulus>{0}</Modulus>";
                publicKeyXml += "<Exponent>{1}</Exponent>";
                publicKeyXml += "</RSAKeyValue>";

                publicKeyXml = string.Format(publicKeyXml, mod, exp);

                RSACryptoServiceProvider publicKey = new RSACryptoServiceProvider();
                publicKey.FromXmlString(publicKeyXml);

                SignedXml signedXml = new SignedXml(doc);

                XmlNodeList nodeList = doc.GetElementsByTagName("Signature");

                signedXml.LoadXml((XmlElement)nodeList[tipoXml == TipoXML.Envio || tipoXml == TipoXML.EnvioBoleta ? nodeList.Count - 1 : 0]);

                return signedXml.CheckSignature(publicKey);
            }
            catch (Exception ex)
            {
                messageOut = ex.ToString();
                return false;
            }
        }

        private const string XPATH_FIRMA_DTE = "sii:DTE/sig:Signature/sig:SignatureValue";
        public static string GetFirmaFromFile(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("sii", doc.DocumentElement.NamespaceURI);
            ns.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");

            return doc.SelectSingleNode(XPATH_FIRMA_DTE, ns).InnerText;
        }

        private const string XPATH_FIRMA_DTE_STRING = "sii:EnvioDTE/sii:SetDTE/sii:DTE/sig:Signature/sig:SignatureValue";
        public static string GetFirmaFromString(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);
            //doc.InnerXml =  doc.InnerXml.Replace(@"xmlns=""""", "").Replace("iso-8859-1", "ISO-8859-1");
            //doc.LoadXml(System.IO.File.ReadAllText(filePath, System.Text.Encoding.GetEncoding("ISO-8859-1")));
            
            try
            {    
                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("sii", "http://www.sii.cl/SiiDte");
                ns.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
                /*doc.InnerXml = RemoveAllNamespaces1(doc.InnerXml);

                string texto = doc.SelectNodes(XPATH_FIRMA_DTE_STRING)[0].InnerText;
                string xmlTexto = doc.SelectNodes(XPATH_FIRMA_DTE_STRING)[0].InnerXml;
                return xmlTexto;*/
                return doc.SelectSingleNode(XPATH_FIRMA_DTE_STRING, ns).InnerText;
            }
            catch (Exception ex)
            {
                string efd = ex.Message;
                return ex.Message;
            }
        }

        public static string RemoveAllNamespaces1(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        /// <summary>
        /// Ruta al valor del Digest de la firma digital de un EnvioDTE.
        /// </summary>
        private const string XPATH_DIGEST_ENVIO = "sii:EnvioDTE/sig:Signature/sig:SignedInfo/sig:Reference/sig:DigestValue";
        private const string XPATH_DIGEST_ENVIO2 = "sii:EnvioDTE/sii:SetDTE/sii:DTE/sig:Signature/sig:SignedInfo/sig:Reference/sig:DigestValue";
        public static string GetDigestValueFromFile(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(filePath);
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("sii", doc.DocumentElement.NamespaceURI);
            ns.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
            try
            {          
                XmlNode nodo = doc.SelectSingleNode(XPATH_DIGEST_ENVIO, ns);
                string retorno = nodo.InnerText;
                return retorno;
            }
            catch 
            {
                XmlNode nodo = doc.SelectSingleNode(XPATH_DIGEST_ENVIO2, ns);
                string retorno = nodo.InnerText;
                return retorno;
            }
        }
    }
}
