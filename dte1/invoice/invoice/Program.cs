using Invoicer.Models;
using Invoicer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.sf.jni4net;
using System.IO;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using java.io;

namespace invoice
{
    class Program
    {
        public static String DEST = "results/zugferd/pdf/basic%05d.pdf";
    public static String ICC = "resources/data/sRGB_CS_profile.icm";
    public static String FONT = "resources/fonts/OpenSans-Regular.ttf";
    public static String FONTB = "resources/fonts/OpenSans-Bold.ttf";
    
    public Font font10;
        public Font font10b;
        public Font font12;
        public Font font12b;
        public Font font14;
        static void Main(string[] args)
        {
            Bridge.CreateJVM(new BridgeSetup());

            File file = new File(DEST);
            file.getParentFile().mkdirs();
            PdfInvoicesBasic app = new PdfInvoicesBasic();
            PojoFactory factory = PojoFactory.getInstance();
            List<Invoice> invoices = factory.getInvoices();
            for (Invoice invoice : invoices)
            {
                app.createPdf(invoice);
            }
            factory.close();
        }


    public void PdfInvoicesBasic() {
        BaseFont bf = BaseFont.createFont(FONT, BaseFont.WINANSI, BaseFont.EMBEDDED);
    BaseFont bfb = BaseFont.createFont(FONTB, BaseFont.WINANSI, BaseFont.EMBEDDED);
    font10 = new Font(bf, 10);
    font10b = new Font(bfb, 10);
    font12 = new Font(bf, 12);
    font12b = new Font(bfb, 12);
    font14 = new Font(bf, 14);
}

public void createPdf(Invoice invoice) {
        String dest = String.format(DEST, invoice.getId());
InvoiceData invoiceData = new InvoiceData();
BasicProfile basic = invoiceData.createBasicProfileData(invoice);

// step 1
Document document = new Document();
// step 2
PdfWriter writer = PdfWriter.getInstance(document, new FileOutputStream(dest), PdfAConformanceLevel.ZUGFeRDBasic);
writer.setPdfVersion(PdfWriter.VERSION_1_7);
writer.createXmpMetadata();
writer.getXmpWriter().setProperty(PdfAXmpWriter.zugferdSchemaNS, PdfAXmpWriter.zugferdDocumentFileName, "ZUGFeRD-invoice.xml");
// step 3
document.open();
// step 4
ICC_Profile icc = ICC_Profile.getInstance(new FileInputStream(ICC));
writer.setOutputIntents("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1", icc);

// header
Paragraph p;
p = new Paragraph(basic.getName() + " " + basic.getId(), font14);
p.setAlignment(Element.ALIGN_RIGHT);
document.add(p);
p = new Paragraph(convertDate(basic.getDateTime(), "MMM dd, yyyy"), font12);
p.setAlignment(Element.ALIGN_RIGHT);
document.add(p);

// Address seller / buyer
PdfPTable table = new PdfPTable(2);
table.setWidthPercentage(100);
PdfPCell seller = getPartyAddress("From:",
        basic.getSellerName(),
        basic.getSellerLineOne(),
        basic.getSellerLineTwo(),
        basic.getSellerCountryID(),
        basic.getSellerPostcode(),
        basic.getSellerCityName());
table.addCell(seller);
PdfPCell buyer = getPartyAddress("To:",
        basic.getBuyerName(),
        basic.getBuyerLineOne(),
        basic.getBuyerLineTwo(),
        basic.getBuyerCountryID(),
        basic.getBuyerPostcode(),
        basic.getBuyerCityName());
table.addCell(buyer);
seller = getPartyTax(basic.getSellerTaxRegistrationID(),
        basic.getSellerTaxRegistrationSchemeID());
table.addCell(seller);
buyer = getPartyTax(basic.getBuyerTaxRegistrationID(),
        basic.getBuyerTaxRegistrationSchemeID());
table.addCell(buyer);
document.add(table);

// line items
table = new PdfPTable(6);
table.setWidthPercentage(100);
table.setSpacingBefore(10);
table.setSpacingAfter(10);
table.setWidths(new int[] { 7, 2, 1, 2, 2, 2 });
table.addCell(getCell("Item:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("Price:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("Qty:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("Subtotal:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("VAT:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("Total:", Element.ALIGN_LEFT, font12b));
Product product;
for (Item item : invoice.getItems())
{
    product = item.getProduct();
    table.addCell(getCell(product.getName(), Element.ALIGN_LEFT, font12));
    table.addCell(getCell(InvoiceData.format2dec(InvoiceData.round(product.getPrice())), Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(String.valueOf(item.getQuantity()), Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(InvoiceData.format2dec(InvoiceData.round(item.getCost())), Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(InvoiceData.format2dec(InvoiceData.round(product.getVat())), Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(
        InvoiceData.format2dec(InvoiceData.round(item.getCost() + ((item.getCost() * product.getVat()) / 100))),
        Element.ALIGN_RIGHT, font12));
}
document.add(table);

// grand totals
document.add(getTotalsTable(
        basic.getTaxBasisTotalAmount(), basic.getTaxTotalAmount(), basic.getGrandTotalAmount(), basic.getGrandTotalAmountCurrencyID(),
        basic.getTaxTypeCode(), basic.getTaxApplicablePercent(),
        basic.getTaxBasisAmount(), basic.getTaxCalculatedAmount(), basic.getTaxCalculatedAmountCurrencyID()));

// payment info
document.add(getPaymentInfo(basic.getPaymentReference(), basic.getPaymentMeansPayeeFinancialInstitutionBIC(), basic.getPaymentMeansPayeeAccountIBAN()));

// XML version
InvoiceDOM dom = new InvoiceDOM(basic);
PdfDictionary parameters = new PdfDictionary();
parameters.put(PdfName.MODDATE, new PdfDate());
PdfFileSpecification fileSpec = writer.addFileAttachment(
        "ZUGFeRD invoice", dom.toXML(), null,
        "ZUGFeRD-invoice.xml", "application/xml",
        AFRelationshipValue.Alternative, parameters);
PdfArray array = new PdfArray();
array.add(fileSpec.getReference());
writer.getExtraCatalog().put(PdfName.AF, array);

// step 5
document.close();
    }
    
    public PdfPCell getPartyAddress(String who, String name, String line1, String line2, String countryID, String postcode, String city)
{
    PdfPCell cell = new PdfPCell();
    cell.setBorder(PdfPCell.NO_BORDER);
    cell.addElement(new Paragraph(who, font12b));
    cell.addElement(new Paragraph(name, font12));
    cell.addElement(new Paragraph(line1, font12));
    cell.addElement(new Paragraph(line2, font12));
    cell.addElement(new Paragraph(String.format("%s-%s %s", countryID, postcode, city), font12));
    return cell;
}
public PdfPCell getPartyTax(String[] taxId, String[] taxSchema)
{
    PdfPCell cell = new PdfPCell();
    cell.setBorder(PdfPCell.NO_BORDER);
    cell.addElement(new Paragraph("Tax ID(s):", font10b));
    if (taxId.length == 0)
    {
        cell.addElement(new Paragraph("Not applicable", font10));
    }
    else
    {
        int n = taxId.length;
        for (int i = 0; i < n; i++)
        {
            cell.addElement(new Paragraph(String.format("%s: %s", taxSchema[i], taxId[i]), font10));
        }
    }
    return cell;
}

public PdfPTable getTotalsTable(String tBase, String tTax, String tTotal, String tCurrency,
        String[] type, String[] percentage, String base[], String tax[], String currency[]) throws DocumentException
{
    PdfPTable table = new PdfPTable(6);
table.setWidthPercentage(100);
table.setWidths(new int[] { 1, 1, 3, 3, 3, 1 });
table.addCell(getCell("TAX", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("%", Element.ALIGN_RIGHT, font12b));
table.addCell(getCell("Base amount:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("Tax amount:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("Total:", Element.ALIGN_LEFT, font12b));
table.addCell(getCell("", Element.ALIGN_LEFT, font12b));
int n = type.length;
for (int i = 0; i < n; i++)
{
    table.addCell(getCell(type[i], Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(percentage[i], Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(base[i], Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(tax[i], Element.ALIGN_RIGHT, font12));
    double total = Double.parseDouble(base[i]) + Double.parseDouble(tax[i]);
    table.addCell(getCell(InvoiceData.format2dec(InvoiceData.round(total)), Element.ALIGN_RIGHT, font12));
    table.addCell(getCell(currency[i], Element.ALIGN_LEFT, font12));
}
PdfPCell cell = getCell("", Element.ALIGN_LEFT, font12b);
cell.setColspan(2);
cell.setBorder(PdfPCell.NO_BORDER);
table.addCell(cell);
table.addCell(getCell(tBase, Element.ALIGN_RIGHT, font12b));
table.addCell(getCell(tTax, Element.ALIGN_RIGHT, font12b));
table.addCell(getCell(tTotal, Element.ALIGN_RIGHT, font12b));
table.addCell(getCell(tCurrency, Element.ALIGN_LEFT, font12b));
return table;
    }
    
    public PdfPCell getCell(String value, int alignment, Font font)
{
    PdfPCell cell = new PdfPCell();
    cell.setUseAscender(true);
    cell.setUseDescender(true);
    Paragraph p = new Paragraph(value, font);
    p.setAlignment(alignment);
    cell.addElement(p);
    return cell;
}

public Paragraph getPaymentInfo(String ref, String[] bic, String[] iban)
{
    Paragraph p = new Paragraph(String.format(
            "Please wire the amount due to our bank account using the following reference: %s",
            ref), font12);
    int n = bic.length;
    for (int i = 0; i < n; i++)
    {
        p.add(Chunk.NEWLINE);
        p.add(String.format("BIC: %s - IBAN: %s", bic[i], iban[i]));
    }
    return p;
}

public String convertDate(Date d, String newFormat) throws ParseException
{
    SimpleDateFormat sdf = new SimpleDateFormat(newFormat);
return sdf.format(d);
    }
}

    }
}
  