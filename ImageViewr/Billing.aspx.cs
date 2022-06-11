using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ImageViewr
{
    public partial class Billing : System.Web.UI.Page
    {
        SqlConnection _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDatabase"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {             
                PopulateDropdown();
                BillingDate.Text = DateTime.Now.ToShortDateString();
                DateFilter.Text = DateTime.Now.ToShortDateString();
            }
        }

        protected void GetUserProductList(int id)
        {      
            
            SqlCommand query = new SqlCommand("exec stp_GetUserPorductList '" + id + "'", _connection);          
            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Item Code");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Cost");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Amount");
            _connection.Open();
            SqlDataReader ProductList = query.ExecuteReader();
            while(ProductList.Read())
            {
                DataRow dr = dt.NewRow();
                dr["S.No"] = ProductList["Id"] ;
                dr["Item Code"] = ProductList["ProductId"];
                dr["Product Name"] = ProductList["Name"];
                dr["Cost"] = ProductList["Price"];
                dr["Quantity"] = ProductList["Quantity"];
                dr["Amount"] = GetProductAmount(Convert.ToInt32(ProductList["Price"]), Convert.ToInt32(ProductList["Quantity"]));
                dt.Rows.Add(dr);
            }
            _connection.Close();
            UserProductList.DataSource = dt;
            UserProductList.DataBind();        
            GetTotalAmount();
            
        }
        public int GetProductAmount(int price,int qty)
        {
            int amount = price * qty;
            return amount;
        }
        public void PopulateDropdown()
        {
            SqlCommand query = new SqlCommand("exec stp_GetAllUser",_connection);
            SqlDataAdapter sd = new SqlDataAdapter(query);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            UserDropDown.DataSource = dt;
            UserDropDown.DataTextField = "FirstName";
            UserDropDown.DataValueField = "Id";
            UserDropDown.DataBind();
            SecondUserDropdownList.DataSource = dt;
            SecondUserDropdownList.DataTextField = "FirstName";
            SecondUserDropdownList.DataValueField ="Id";
            SecondUserDropdownList.DataBind();
            SqlCommand ProductQuery = new SqlCommand("exec stp_GetAllProduct", _connection);
            SqlDataAdapter _sd = new SqlDataAdapter(ProductQuery);
            DataTable _dt = new DataTable();
            _sd.Fill(_dt);
            ProductDropdown.DataSource = _dt;
            ProductDropdown.DataTextField = "Name";
            ProductDropdown.DataValueField = "ProductId";
            ProductDropdown.DataBind();
           
        }

        protected void AddProduct_Click(object sender, EventArgs e)
        {     
            int id = Convert.ToInt32(UserDropDown.SelectedValue.ToString());
            int productId = Convert.ToInt32(ProductDropdown.SelectedValue.ToString());
            int quantity =Convert.ToInt32(Quantity.Text);
            if (quantity > 0)
            {
                SqlCommand query = new SqlCommand("exec stp_AddProductToCart '" + id + "','" + productId + "','" + quantity + "'", _connection);
                _connection.Open();
                query.ExecuteNonQuery();
                _connection.Close();
                GetUserProductList(Convert.ToInt32( UserDropDown.SelectedValue));
            }
            else userMsg.Text = "Quantity cannot be less then 1";
            
            GetUserProductList(Convert.ToInt32(UserDropDown.SelectedValue));
           
        }
        public void GetTotalAmount()
        {
            int total = 0;
            for (int i =0;i < UserProductList.Rows.Count;i++)
            {
                total += Convert.ToInt32(UserProductList.Rows[i].Cells[4].Text);
            }
            float gst = (total / 100) * 10;
            Amount.Text ="Total : INR "+ total.ToString() +"/-";
            Gst.Text = "10% GST* : INR " + gst + "/-";
            TotalAmount.Text = "Grand Total : INR "+ Convert.ToString(gst+total) + "/-";           
        }

        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            int UserId =Convert.ToInt32( UserDropDown.SelectedValue.ToString());
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            int subTotal = Convert.ToInt32(TotalAmount.Text.Replace("Grand Total : INR ","").Replace("/-",""));
            int TransectionId =  GenerateTransection(UserId,date,subTotal); //Generate Transection and return it's Id
            //Add product to the list details
            for (int i = 0; i < UserProductList.Rows.Count; i++)
            {
                int productId = Convert.ToInt32(UserProductList.Rows[i].Cells[2].Text);
                int quantity = Convert.ToInt32(UserProductList.Rows[i].Cells[5].Text);
                AddProductToTransectionDetails(productId,TransectionId,quantity);
            }
            DeleteCartItem(UserId); //Clear Current item list
            GetUserProductList(UserId);


        }

        private int GenerateTransection(int UserId,string Date,int total)
        {
            SqlCommand query = new SqlCommand("exec stp_AddTransection '"+UserId+"','"+total+"','"+Date+"'",_connection);
            _connection.Open();
            int Transectionid = Convert.ToInt32(query.ExecuteScalar().ToString());
            _connection.Close();
            return Transectionid;
        }
        private  void AddProductToTransectionDetails(int ProductId,int TransectionId,int quantity)
        {
            SqlCommand query = new SqlCommand("exec stp_AddTransectionDetails '" + ProductId + "','" + TransectionId + "','" + quantity + "'", _connection);
            _connection.Open();
            query.ExecuteNonQuery();
            _connection.Close();
        }
        private void DeleteCartItem(int UserId)
        {
            SqlCommand query = new SqlCommand("exec stp_DeleteProductListOnCart '" + UserId + "'", _connection);
            _connection.Open();
            query.ExecuteNonQuery();
            _connection.Close();
        }

        protected void UserDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetUserProductList(Convert.ToInt32(UserDropDown.SelectedValue.ToString()));
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            ClearDataGrid();

            int UserId = Convert.ToInt32(SecondUserDropdownList.SelectedValue.ToString());
            GetCustomerName(UserId);
            string checkDate = DateFilter.Text;
            if (checkDate == "")
            {
                GetTransectionList(UserId);
            }
            else
            {
                DateTime dt = Convert.ToDateTime(DateFilter.Text);
                string date = dt.ToString("yyyy-dd-MM");
                GetTransectionListWithFilter(UserId, date);
            }
           
        }
        private void ClearDataGrid()
        {
            DataTable ds = new DataTable();
            ds = null;
            TransectionDataList.DataSource = ds;
           TransectionDataList.DataBind();
            BilledProductList.DataSource = ds;
            BilledProductList.DataBind();
        }
        private void GetCustomerName(int UserId)
        {
            SqlCommand cmd = new SqlCommand("exec stp_GetUser '"+UserId+"'",_connection);
            _connection.Open();
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                UserNamelbl.Text = "Customer : " + dr["FirstName"] + " " + dr["LastName"];
            }
            _connection.Close();
        }
        private void GetTransectionList(int UserId)
        {
            SqlCommand query = new SqlCommand(" exec stp_GetTransectioOfUser '"+UserId+"'", _connection);         
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Total Amount");
            dt.Columns.Add("Date");
            _connection.Open();
            SqlDataReader TransectionList = query.ExecuteReader();
            if (TransectionList != null)
            {
                while (TransectionList.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = TransectionList["Id"];
                    dr["Total Amount"] = TransectionList["TotalAmount"];
                    dr["Date"] = Convert.ToString(TransectionList["Date"]).Replace("12:00:00 AM", "");
                    dt.Rows.Add(dr);

                }
                _connection.Close();
                TransectionDataList.DataSource = dt;
                TransectionDataList.DataBind();
            }
            else errMsg.Text = "Cannot find the record of specified User";
          
        }
        private void GetTransectionListWithFilter(int UserId,string date)
        {
            SqlCommand query = new SqlCommand(" exec stp_GetTransectioOfUserWithFilter '" + UserId + "','"+date+ "'", _connection);
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Total Amount");
            dt.Columns.Add("Date");
            _connection.Open();
            SqlDataReader TransectionList = query.ExecuteReader();
            if (TransectionList.HasRows)
            {
                while (TransectionList.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = TransectionList["Id"];
                    dr["Total Amount"] = TransectionList["TotalAmount"];
                    dr["Date"] = Convert.ToString(TransectionList["Date"]).Replace("12:00:00 AM", "");
                    dt.Rows.Add(dr);

                }
                _connection.Close();
                TransectionDataList.DataSource = dt;
                TransectionDataList.DataBind();
            }
            else errMsg.Text = "Cannot find record of specified date";

        }

        protected void TransectionDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int transectionId = Convert.ToInt32(TransectionDataList.SelectedRow.Cells[1].Text);
            GetBilledProduct(transectionId);
        }
        private void GetBilledProduct(int TransectId)
        {
            SqlCommand query = new SqlCommand(" exec stp_GetTransectionDetail '" + TransectId + "'", _connection);
            SqlDataAdapter sd = new SqlDataAdapter(query);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            BilledProductList.DataSource= dt;
            BilledProductList.DataBind();

        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            DateFilter.Text = "";
        }

        protected void UserProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ProductListId = Convert.ToInt32(UserProductList.SelectedRow.Cells[1].Text);
            RemoveItemFromCart(ProductListId);

        }
        public void RemoveItemFromCart(int ListId)
        {
            SqlCommand query = new SqlCommand("exec stp_DeleteProductFromCart '" + ListId + "'", _connection);
            _connection.Open();
            query.ExecuteNonQuery();
            _connection.Close();
            int Id = Convert.ToInt32(UserDropDown.SelectedValue.ToString());
            GetUserProductList(Id);
        }
    }
    }
