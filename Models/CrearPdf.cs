using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProyectoGrupo5.Models
{
    public class CrearPdf
    {
        public string CrearFactura(string usuario, List<Ventas> Carrito) {
            decimal total = 0;
            string Direccion = usuario + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("Facturas/" + Direccion + ".pdf", FileMode.Create));
            doc.Open();

            // Encabezado de la factura
            Paragraph header = new Paragraph();
            header.Add(new Phrase("FrutaVerdeMarket.Com\n\n", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
            header.Add(new Phrase("Cliente:" + usuario, new Font(Font.FontFamily.HELVETICA, 15, Font.BOLD)));
            header.Add(new Phrase("\n\nFecha:" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm")+ "\n\n", new Font(Font.FontFamily.HELVETICA, 15, Font.BOLD)));
            header.Alignment = Element.ALIGN_CENTER;
            doc.Add(header);

            // Tabla para los detalles de la factura
            PdfPTable table = new PdfPTable(5);
            float[] columnWidths = { 30f, 30f, 15f, 15f, 15f };
            table.SetWidths(columnWidths);

            // Encabezados de la tabla
            table.AddCell(new PdfPCell(new Phrase("Producto", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Descripción", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Precio Unitario", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Cantidad", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))));

            // Detalles de la factura
            foreach (var venta in Carrito)
            {
                table.AddCell(venta.Productos.Nombre);
                table.AddCell(venta.Productos.Descripcion);
                table.AddCell((venta.Total / venta.Cantidad).ToString("C2")); // Formatear precio como moneda
                table.AddCell(venta.Cantidad.ToString());
                table.AddCell(venta.Total.ToString("C2")); // Formatear total como moneda
                total += venta.Total;
            }

            // Agregar la tabla a la factura
            doc.Add(table);

            // Pie de página
            Paragraph footer = new Paragraph();
            footer.Add(new Phrase("\n\nTotal De La Compra: " + total.ToString("C2"), new Font(Font.FontFamily.HELVETICA, 12)));
            footer.Add(new Phrase("\n\nGracias por su compra.", new Font(Font.FontFamily.HELVETICA, 12)));
            footer.Alignment = Element.ALIGN_RIGHT;
            doc.Add(footer);

            doc.Close();

            return Direccion;

        }
    }
}
