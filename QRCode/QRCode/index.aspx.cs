using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;

namespace QRCode
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            var guid = Guid.NewGuid();
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            var result = writer.Write(txtCode.Text);
            string path = Server.MapPath("~/images/QRImage_" + guid + ".jpg");
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using(FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            imgQRCode.Visible = true;
            imgQRCode.ImageUrl = "~/images/QRImage_" + guid + ".jpg";
            imgQRCode.Attributes.Add("imagename", "QRImage_" + guid + ".jpg");
            Session["imagename"] = "QRImage_" + guid + ".jpg";
        }
        protected void btnRead_Click(object sender, EventArgs e)
        {
            var reader = new BarcodeReader();

            string filename = Path.Combine(Request.MapPath("~/images"), Session["imagename"].ToString());
            // Detect and decode the barcode inside the bitmap
            var result = reader.Decode(new Bitmap(filename));
            if (result != null)
            {
                lblQRCode.Text = "QR Code: " + result.Text;
            }
        }






    }
}