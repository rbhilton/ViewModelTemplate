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
            List<Customer> customers = db.customers.ToList();
            return customers;
        }

        /***** Use EF to get the customer orders *****/
        public CustomerOrders getCustomerOrdersEF(string custNo)
        {
            CustomerOrders customerOrders = new CustomerOrders();
            OrderEntryDbContext db = new OrderEntryDbContext();
            customerOrders.customer = db.customers.Find(custNo);
            var query = (from ot in db.orders where ot.CustNo == custNo select ot);
            customerOrders.orders = query.ToList();

            return customerOrders;
        }

        /***** Use SQL to get the customer orders *****/
        public CustomerOrders getCustomerOrdersSQL(string custNo)
        {
            CustomerOrders customerOrders = new CustomerOrders();
            OrderEntryDbContext db = new OrderEntryDbContext();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@CustNo", custNo));

            string sql = "SELECT * FROM Customer WHERE CustNo = @CustNo";
            customerOrders.customer =
                db.customers.SqlQuery(sql, sqlParams.ToArray()).First();

            sqlParams.Clear();
            sqlParams.Add(new SqlParameter("@CustNo", custNo));
            sql = "SELECT * FROM OrderTbl WHERE CustNo = @CustNo";
            customerOrders.orders =
                db.orders.SqlQuery(sql, sqlParams.ToArray()).ToList();

            return customerOrders;
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

}