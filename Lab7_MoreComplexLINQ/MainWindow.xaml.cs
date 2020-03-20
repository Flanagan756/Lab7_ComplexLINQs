using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab7_MoreComplexLINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        NORTHWNDEntities db = new NORTHWNDEntities();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1.Display number of customers per country, sorted descending by the number.
            var query1 = from c in db.Customers
                         group c by c.Country into countryGroup
                         orderby countryGroup.Count() descending
                         select new
                         {
                             Country = countryGroup.Key,
                             Count = countryGroup.Count(),
                         };

            var results1 = query1.ToList();
            dg1.ItemsSource = results1;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //2.	Show customers from Italy.
            var query2 = from c in db.Customers
                         where c.Country == "Italy"
                         select new
                         {
                             c.Orders,
                             c.CustomerDemographics,
                             c.CustomerID,
                             c.CompanyName,
                             c.ContactName

                         };

            var results2 = query2.ToList();
            dg2.ItemsSource = results2;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //1. For each product, display information if the product is available.
            var query3 = from p in db.Products
                         where p.UnitsInStock > 0
                         select new
                         {
                             p.ProductName,
                             Available = p.UnitsInStock
                         };

            var results3 = query3.ToList();
            dg3.ItemsSource = results3;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //4.List all discounted products from all orders.
            var query4 = from od in db.Order_Details
                         join p in db.Products on od.ProductID equals p.ProductID
                         orderby p.ProductName ascending
                         where od.Discount > 0
                         select new
                         {
                             p.ProductName,
                             DiscountGiven = od.Discount,
                             od.OrderID
                         };

            var results4 = query4.ToList();
            dg4.ItemsSource = results4;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //5.Calculate the total freight over all orders.
            //Use a textblock to show the results.
            var query5 = (from o in db.Orders
                          select o.Freight).Sum();


            txtblk5.Text = string.Format("The total value of freight for all orders is {0:c}", query5);



        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //6.Products and Categories in order of category name
            //with the highest priced product in each category at the top of the list
            var query6 = from p in db.Products
                         join c in db.Categories on p.CategoryID equals c.CategoryID
                         orderby c.CategoryName ascending, p.UnitPrice descending
                         select new
                         {

                             c.CategoryName,
                             p.ProductName,
                             p.UnitPrice

                         };

            var results6 = query6.ToList();
            dg6.ItemsSource = results6;


        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //7.	Top 10 customers grouped by number of orders
            var query7 = (from o in db.Orders
                          group o by o.CustomerID into numberOfOrders
                          orderby numberOfOrders.Count() descending
                          select new
                          {

                              CustomerID = numberOfOrders.Key,
                              NumberOfOrders = numberOfOrders.Count(),


                          }).Take(10);

            var results7 = query7.ToList();
            dg7.ItemsSource = results7;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            //8.	List numbers of orders grouped by customers.  
            //      This is an amendment of above with a join added in.

            var query8 = (from o in db.Orders                      
                          group o by o.CustomerID into numberOfOrders
                          join c in db.Customers on numberOfOrders.Key equals c.CustomerID
                          orderby numberOfOrders.Count() descending
                          select new
                          {

                              CustomerID = numberOfOrders.Key,
                              CompanyName = c.CompanyName,
                              NumberOfOrders = numberOfOrders.Count(),


                          }).Take(10);
            

            var results8 = query8.ToList();
            dg8.ItemsSource = results8;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //9. List customers without orders.
            var query9 = from c in db.Customers
                         where c.Orders.Count == 0                        
                         select new
                         {
                           c.CompanyName,
                           NumberofOrders = c.Orders

                         };


            var results9 = query9.ToList();
            dg9.ItemsSource = results9;
        }
    }
    
}
