using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageViewr;

namespace ImageViewr
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDatabase"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAllImages();
            }
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            UploadImage();
        }


        protected void UploadImage()
        {
            try
            {
                string originalFilenName = Path.GetFileName(ImageSelector.FileName);
                //Check if the file is selected or not 
                if (ImageSelector.HasFile)
                {
                    string fileExtension = Path.GetExtension(ImageSelector.FileName).ToLower();
                    string date = DateTime.Now.ToShortDateString();
                    //Check the file type
                    if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".jpeg")
                    {
                        string filepath = Server.MapPath("\\CDN\\Image\\" + date + "\\Original\\");
                        string filepathfordb = "\\CDN\\Image\\" + date + "\\Original\\";
                        //check if directory exist or not
                        if (!Directory.Exists(filepath))
                        {
                            Directory.CreateDirectory(filepath);
                        }
                        string newFileName = SaveImageToDB(filepathfordb, originalFilenName, fileExtension);
                        ImageSelector.SaveAs(filepath + newFileName);
                        ThumbnailGenerateor(filepath, newFileName);
             //           successMsg.Text = "Successfully Uploaded !";
                        GetAllImages();
                    }
            //        else msg.Text = "File must be png/jpg/jpeg";
                }
            //    else msg.Text = "Please Select a file";
            }
            catch (Exception ex)
            {
         //       msg.Text = ex.Message;
            }
        }

        public void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            getFilePath(Convert.ToInt32(ImageData.SelectedRow.Cells[1].Text));
        }



        //Method to store file in Database
        protected string SaveImageToDB(string path, string filename, string filetype)
        {
            DateTime date = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            //ADD FILE TO DATABASE
            SqlCommand query = new SqlCommand("exec stp_AddImage '" + filename + "','" + path + "','" + date + "'", _connection);
            _connection.Open();
            int id = Convert.ToInt32(query.ExecuteScalar().ToString());
            _connection.Close();

            string NameDate = DateTime.Now.ToString("yyyyMMddhhmmss");
            string Newfilename = id + "-" + NameDate + filetype;
            string newPath = path + Newfilename;

            // Update File Path to DATABASE
            UpdatePath(id, newPath);
            return Newfilename;
        }
        protected void UpdatePath(int id, string newPath)
        {
            _connection.Open();
            SqlCommand UpdateQuery = new SqlCommand("exec stp_UpdateImagePath '" + id + "','" + newPath + "'", _connection);
            UpdateQuery.ExecuteNonQuery();
            _connection.Close();
        }
        protected void GetAllImages()
        {
            SqlCommand query = new SqlCommand(" exec stp_GetAllIamges", _connection);
            SqlDataAdapter sd = new SqlDataAdapter(query);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dt.Columns.Remove("FilePath");
            dt.Columns.Remove("UploadDate");
            ImageData.DataSource = dt;
            ImageData.DataBind();
        }


        protected void getFilePath(int id)
        {
            SqlCommand query = new SqlCommand(" exec stp_GetImageWithId '" + id + "'", _connection);
            _connection.Open();
            SqlDataReader datareader;
            datareader = query.ExecuteReader();
            
            if(datareader.Read())
            {                       
                string path = datareader["FilePath"].ToString();          
                ImageOrg.ImageUrl = path;
                string thumbnailPart = path.Replace("Original", "Thumbnail");
                Thumbnail.ImageUrl = thumbnailPart;
                Title.Text = datareader["FileName"].ToString();
                UploadDate.Text = datareader["UploadDate"].ToString().Replace("12:00:00 AM","");
                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(path));
                width.Text = img.Width.ToString()+"px";
                height.Text = img.Height.ToString()+"px";
                var fileLength = new FileInfo(Server.MapPath(path)).Length;
                float sizeofFile = Convert.ToInt32(fileLength) / 1000000;
                size.Text = sizeofFile.ToString()+"Mb";           
            }
            _connection.Close();

        }

        protected void ThumbnailGenerateor(string filepath, string name)
        {
            Imager.PerformImageResizeAndPutOnCanvas(filepath, name, name);
        }
    }
}
