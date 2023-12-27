using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ITA_CHILE.Security.Timbre
{
    public static class Timbre
    {
        private static bool verbose = false;

        /// <summary>
        /// Timbra un documento.
        /// </summary>
        /// <param name="text">Texto que se desea timbrar.</param>
        /// <param name="privateKey">LLave privada con la que se desea timbrar "text".</param>
        /// <returns>String que representa el texto timbrado.</returns>
        public static string Timbrar(string text, string privateKey)
        {
            Encoding ByteConverter = Encoding.GetEncoding("ISO-8859-1");
            //Encoding ByteConverter = Encoding.UTF8;
            byte[] textEnBytes = ByteConverter.GetBytes(text);
            byte[] HashValue = new SHA1CryptoServiceProvider().ComputeHash(textEnBytes);

            RSACryptoServiceProvider rsa = Timbre.crearRsaDesdePEM(privateKey);
            byte[] bytesSing = rsa.SignHash(HashValue, "SHA1");
            string firma = Convert.ToBase64String(bytesSing);

            if (VerificarTimbre(text, privateKey, firma))
                return firma;
            else return "";
        }

        /// <summary>
        /// Dado un texto y una llave privada, verifica que el timbre esté correcto.
        /// </summary>
        /// <param name="text">Texto que se desea comprobar.</param>
        /// <param name="privateKey">Llave privada utilizada para timbrar el texto.</param>
        /// <param name="resultado">Resultado sobre el cual se desea comprobar.</param>
        /// <returns>Verdadero si el timbre es válido, falto de otra manera.</returns>
        public static bool VerificarTimbre(string text, string privateKey, string resultado)
        {
            //Encoding ByteConverter = Encoding.UTF8;
            Encoding ByteConverter = Encoding.GetEncoding("ISO-8859-1");
            byte[] textEnBytes = ByteConverter.GetBytes(text);
            byte[] HashValue = new SHA1CryptoServiceProvider().ComputeHash(textEnBytes);

            RSACryptoServiceProvider rsa = Timbre.crearRsaDesdePEM(privateKey);

            byte[] bytesSing = rsa.SignHash(HashValue, "SHA1");

            string firma = Convert.ToBase64String(bytesSing);

            return resultado.Equals(firma);
        }

        public static RSACryptoServiceProvider crearRsaDesdePEM(string base64)
        {
            base64 = base64.Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty);
            base64 = base64.Replace("-----END RSA PRIVATE KEY-----", string.Empty);

            byte[] arrPK = Convert.FromBase64String(base64);

            return DecodeRSAPrivateKey(arrPK);
        }

        public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();        
                else if (twobytes == 0x8230)
                    binr.ReadInt16();        
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);
                
                if (verbose)
                {
                    Console.WriteLine("showing components ..");

                    showBytes("\nModulus", MODULUS);
                    showBytes("\nExponent", E);
                    showBytes("\nD", D);
                    showBytes("\nP", P);
                    showBytes("\nQ", Q);
                    showBytes("\nDP", DP);
                    showBytes("\nDQ", DQ);
                    showBytes("\nIQ", IQ);
                }

                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)            
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    
            else
                if (bt == 0x82)
                {
                    highbyte = binr.ReadByte();    
                    lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                }
                else
                {
                    count = bt;
                }

            while (binr.ReadByte() == 0x00)
            {   
                count -= 1;
            }

            binr.BaseStream.Seek(-1, SeekOrigin.Current);          
            return count;
        }

        private static void showBytes(String info, byte[] data)
        {
            Console.WriteLine("{0} [{1} bytes]", info, data.Length);
            for (int i = 1; i <= data.Length; i++)
            {
                Console.Write("{0:X2} ", data[i - 1]);
                if (i % 16 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }
    }
}
