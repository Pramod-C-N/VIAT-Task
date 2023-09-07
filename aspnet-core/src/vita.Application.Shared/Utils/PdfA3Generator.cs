using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.XMP;
using iText.Pdfa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace vita.Utils
{
    public class PdfA3Generator
    {
        public static void CreatePdf(byte[] doc, byte[] embededFile, string baseUri, string dest, string invoiceno,string orientation,bool isDraft=false)
        {

            //-------------important-----------------------------
            //LicenseKey.LoadLicenseFile(new FileInfo("wwwroot/license/itextkey.json"));
            //------------valid till 22/12/22--------------------


            try
            {
                PdfWriter pdfwriter = new PdfWriter(dest);
                PdfADocument document = null;
                using (var stream = new FileStream(@"wwwroot\ReportTemplate\sRGB_CS_profile.icm", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    document = new PdfADocument(pdfwriter, PdfAConformanceLevel.PDF_A_3A, new PdfOutputIntent("Custom", "",
                    "https://www.color.org", "sRGB IEC61966-2.1", stream));
                }

                document.SetTagged();
                document.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
                PdfDocumentInfo info = document.GetDocumentInfo();
                info.SetTitle("Invoice");
                info.SetSubject("Invoice");
                info.SetAuthor("Abylle");
                info.AddModDate();
                info.AddCreationDate();
                info.SetProducer("Abylle");

                XMPMeta xmpMeta = XMPMetaFactory.Create();
                //xmpMeta.SetProperty(XMPConst.)
                document.SetXmpMetadata(xmpMeta);

                var parameters = new PdfDictionary();

                parameters.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
                parameters.Put(PdfName.CreationDate, new PdfDate().GetPdfObject());
                if (!isDraft)
                {
                    PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(document, embededFile, "Invoice.xml", "Invoice.xml", parameters, PdfName.Data);
                    fileSpec.Put(new PdfName("AFRelationship"), new PdfName("Data"));
                    document.AddFileAttachment("Invoice", fileSpec);
                    PdfArray array = new PdfArray();
                    array.Add(fileSpec.GetPdfObject().GetIndirectReference());
                    document.GetCatalog().Put(new PdfName("AF"), array);
                }
                if (orientation == "L")
                {
                    document.SetDefaultPageSize(PageSize.A3.Rotate());
                }
                
                ConverterProperties properties = new ConverterProperties();
                properties.SetBaseUri(baseUri);
                DefaultFontProvider fontProvider = new DefaultFontProvider(true, true, true);


                properties.SetFontProvider(fontProvider);
                HtmlConverter.ConvertToPdf(new MemoryStream(doc), document, properties); //define font for whole html file 
                document.Close();
                pdfwriter.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
