using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using ViewModelTemplate.Models;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace ViewModelTemplate.Models
{
    public class DBRepository
    {
      /*************  Repository Methods ******************/

        public List<Customer> getCustomers()
        {
            OrderEntryDbContext db = new OrderEntryDbContext();
            List<Customer> customers = new List<Customer>();
            try
            {
                customers = db.customers.ToList();
            } catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            return customers;
        }

        /***** Use EF to get the customer orders *****/
        public CustomerOrders getCustomerOrdersEF(string custNo)
        {
            CustomerOrders customerOrders = new CustomerOrders();
            OrderEntryDbContext db = new OrderEntryDbContext();
            try
            {
                customerOrders.customer = db.customers.Find(custNo);
                var query = (from ot in db.orders where ot.CustNo == custNo select ot);
                customerOrders.orders = query.ToList();
            } catch (Exception ex) { Console.WriteLine(ex.Message); }

            return customerOrders;
        }

        /***** Use SQL to get the customer orders *****/
        public CustomerOrders getCustomerOrdersSQL(string custNo)
        {
            CustomerOrders customerOrders = new CustomerOrders();
            OrderEntryDbContext db = new OrderEntryDbContext();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@CustNo", custNo));

            try
            {
                string sql = "SELECT * FROM Customer WHERE CustNo = @CustNo";
                customerOrders.customer =
                    db.customers.SqlQuery(sql, sqlParams.ToArray()).First();

                sqlParams.Clear();
                sqlParams.Add(new SqlParameter("@CustNo", custNo));
                sql = "SELECT * FROM OrderTbl WHERE CustNo = @CustNo";
                customerOrders.orders =
                    db.orders.SqlQuery(sql, sqlParams.ToArray()).ToList();
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return customerOrders;
        }

        // Get the list of products for a particular order, for a particular customer.
        public ProductDetailsList getProductDetailsList(string ordNo)
        {
            CustomerOrders customerOrders = new Models.CustomerOrders();
            OrderEntryDbContext db = new OrderEntryDbContext();
            ProductDetailsList prodDetailsList = new ProductDetailsList();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@OrdNo", ordNo));
            //sqlParams.Add(new SqlParameter("@CustNo", custNo));

            try
            {
                // Create the query for all orders for a customer.
                string sql =
                    "SELECT P.ProdNo, ProdName, Qty, ProdPrice " +
                    "FROM Customer C " +
                    "INNER JOIN OrderTbl OTbl " +
                    "ON C.CustNo = OTbl.CustNo " +
                    "INNER JOIN OrdLine OL " +
                    "ON OTbl.OrdNo = OL.OrdNo " +
                    "INNER JOIN Product P " +
                    "ON OL.ProdNo = P.ProdNo " +
                    "WHERE OTbl.OrdNo = @OrdNo";

                // Put the results of sql into a list
                /*foreach (var item in db.Database.SqlQuery<ProductDetails>(sql, sqlParams.ToArray()).ToList())
                {
                    ProductDetails prodDetail = new ProductDetails()
                    {
                        ProdNo = item.ProdNo,
                        ProdName = item.ProdName,
                        Qty = item.Qty,
                        ProdPrice = item.ProdPrice
                    };
                    prodDetailsList.productDetailsList.Add(prodDetail);
                }*/
                // Or:
                prodDetailsList.productDetailsList = db.Database.SqlQuery<ProductDetails>(sql, sqlParams.ToArray()).ToList();
                prodDetailsList.OrdNo = ordNo;
            } catch (Exception ex) { Console.WriteLine(ex.Message); }
            return prodDetailsList;
        }
    }
    /***************** View Models **********************/

    public class CustomerOrders
    {
        public CustomerOrders()
        {
            this.customer = new Customer();
            this.orders = new List<OrderTbl>();
        }

        [Key]
        public string custNo { get; set; }
        public Customer customer { get; set; }
        public List<OrderTbl> orders { get; set; }
    }

    public class ProductDetails
    {
        public ProductDetails()
        {
            this.ProdNo = "";
            this.ProdName = "";
            this.Qty = 0;
            this.ProdPrice = 0.0M;
        }

        [Key]
        [Display(Name = "Product ID")]
        public string ProdNo { get; set; }

        [Display(Name = "Name")]
        public string ProdName { get; set; }

        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        [Display(Name = "Price")]
        public decimal ProdPrice { get; set; }
    }

    public class ProductDetailsList
    {
        public List<ProductDetails> productDetailsList { get; set; }

        public string OrdNo { get; set; }

        public ProductDetailsList()
        {
            this.productDetailsList = new List<ProductDetails>();
        }
    }

}