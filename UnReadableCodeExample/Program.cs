using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnReadableCodeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("For quoting: GetQuote,UserName,ItemName");
                Console.WriteLine("For booking from quote: PlaceShipment,UserName,quote_id");
                Console.WriteLine("Q for Quit.");
                try
                {
                    string line = Console.ReadLine();
                    if (line == "Q")
                    {
                        break;
                    }
                    line = "," + line;
                    args = line.Split(',').Select(x => x.Trim(' ')).ToArray();
                }
                catch
                {
                    //come on!!!!
                }
                try
                {
                    MakeItSoNumberOne(args);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("D'oh!");
                }
            } while (true);
        }

        private static void MakeItSoNumberOne(string[] args)
        {
            if (args[1] == "GetQuote")
            {

                int userId = 0;
                int accountId = 0;
                SqlConnection conn = null;
                SqlCommand cmd = null;
                SqlDataReader rdr = null;
                QuoteService q = null;

                string userQuery = "SELECT userId ";
                userQuery += "FROM Users ";
                userQuery += "WHERE userNmae = '" + args[2] + "'";

                conn = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=UnReadableCodeDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                cmd = new SqlCommand(userQuery, conn);

                //open DB connection
                conn.Open();


                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        userId = (int)rdr.GetValue(0);
                    }
                }
                rdr.Close();

                if (userId > 0)
                {
                    cmd.CommandText = null;
                    rdr = null;
                    string accountQuery = "SELECT accountId ";
                    accountQuery += "FROM Accounts ";
                    accountQuery += "WHERE userId = " + userId.ToString();

                    cmd.CommandText = accountQuery;
                    rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            accountId = rdr.GetInt32(0);
                        }
                    }
                    rdr.Close();
                    
                    if (accountId > 0)
                    {
                        q = new QuoteService();
                        q.IntAccountID = accountId;
                        Quote qt = q.GetQuote(args[3].ToString(), userId.ToString());
                        if (qt != null && qt.Cost > 0 & qt.Item == args[3].ToString())
                        {
                            Console.WriteLine("Congratulations, you have successfully quoted an item!");
                            Console.WriteLine("Your quoteID is " + qt.ID.ToString());
                            Console.WriteLine("use your quoteId when booking shipment to get this price.");
                            Console.WriteLine("this quote will expire in 5 days.");
                        }
                    }
                }


                //don't forget to close the DB connection
                conn.Close();
                conn.Dispose();

                //and dispose the command
                cmd.Dispose();

            }
            else if (args[1] == "PlaceShipment")
            {
                int userId = 0;
                int quoteId = 0;
                int accountId = 0;
                SqlConnection conn = null;
                SqlCommand cmd = null;
                SqlDataReader rdr = null;
                QuoteService q = null;
                ShippingService s = null;
                //first see if we have a quote id
                quoteId = int.Parse(args[3]);

                //lookup the quotes from the quotes table


                string userQuery = "SELECT userId ";
                userQuery += "FROM Users ";
                userQuery += "WHERE userNmae = '" + args[2] + "'";

                conn = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=UnReadableCodeDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                cmd = new SqlCommand(userQuery, conn);

                //open DB connection
                conn.Open();


                rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        userId = (int)rdr.GetValue(0);
                    }
                }
                rdr.Close();

                if (userId > 0)
                {
                    cmd.CommandText = null;
                    rdr = null;
                    string accountQuery = "SELECT accountId ";
                    accountQuery += "FROM Accounts ";
                    accountQuery += "WHERE userId = '" + userId.ToString() + "'";

                    cmd.CommandText = accountQuery;
                    rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            accountId = rdr.GetInt32(0);
                        }
                    }
                    rdr.Close();

                    if (accountId > 0)
                    {
                        q = new QuoteService();
                        q.IntAccountID = accountId;
                        Quote qt = q.GetQuote(args[3].ToString(), userId.ToString());
                        if (qt != null && qt.Cost > 0 & qt.Item == args[2].ToString())
                        {
                            //we now have a quote

                            //next we have to see if the quote is expired
                            if (qt.date <= DateTime.Now.AddDays(5) || ((accountId == 44 || accountId == 102) && qt.date <= DateTime.Now.AddDays(30)))
                            {
                                //if the quote is not good or not found, we need to get another quote and book the shipment with the new quote
                                Console.WriteLine("Quote is expired, please qoute again and use new quote id to book shipment.");

                            }
                            else
                            {

                                //if the quote is good, then we book the shipment with the quoted price
                                //book the shipment from the quoted price
                                s = new ShippingService();
                                Shipment sh = s.BookShipmentFromQuote(qt);
                                Console.WriteLine("Shipment booked, Shipment id == " + sh.shipment_id);
                                Console.WriteLine("Keep this for your records");
                            }
                        }
                    }
                }


                //don't forget to close the DB connection
                conn.Close();
                conn.Dispose();

                //and dispose the command
                cmd.Dispose();
            }
        }
    }

    internal class Shipment
    {
        internal int shipment_id;

        public Exception Errors { get; internal set; }
        public bool HasErrors { get; internal set; }
    }

    internal class ShippingService
    {
        internal Shipment BookShipmentFromQuote(Quote qt)
        {
            if(qt != null && qt.Cost > 0 && qt.ID > 0 && qt.item_id > 0)
            {
                try
                {
                    return new Shipment
                    {
                        shipment_id = new Random().Next(),
                    };
                }
                catch(Exception ex)
                {

                    return new Shipment
                    {
                        HasErrors = true,
                        Errors = ex,
                    };
                }
            }
            else
            {
                Console.WriteLine("Could not book shipment");

                //need to get the exception message for why we couldn't book the shipment
                string message = "";
                if (qt == null)
                {
                    message = "no Quote given."; 
                }
                else if(qt != null)
                {
                    if(qt.Cost == 0)
                    {
                        message = "no cost\r\n";
                    }
                    if(qt.ID <= 0)
                    {
                        message += "quote not saved\r\n";
                    }
                    if(qt.item_id <= 0 || qt.item_id == 100000)
                    {
                        message += "item not provided or not shippable\r\n";
                    }
                }

                return new Shipment
                {
                    HasErrors = true,
                    Errors = new Exception(message),
                };
            }
        }
    }

    /// <summary>
    /// a quote
    /// </summary>
    internal class Quote
    {
        internal DateTime date;

        /// <summary>
        /// the cost
        /// </summary>
        public decimal Cost { get; internal set; }
        /// <summary>
        /// the quote_id
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// the item
        /// </summary>
        public string Item { get; internal set; }
        /// <summary>
        /// the item_id
        /// </summary>
        public int item_id { get; internal set; }
    }

    /// <summary>
    /// GetsQuotes
    /// </summary>
    internal class QuoteService
    {
        /// <summary>
        /// Gets Quotes
        /// </summary>
        public QuoteService()
        {
        }
        /// <summary>
        /// The Integer32 Account ID
        /// </summary>
        public int IntAccountID { get; internal set; }

        /// <summary>
        /// Get A Quote
        /// </summary>
        /// <param name="v">the item</param>
        /// <returns>the quote</returns>
        internal Quote GetQuote(string v, string uAcc)
        {
            Quote q;
            Random r;
            int c;

            q = new Quote();
            r = new Random();
            c = 0;


            //first look up the item in the database...
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader rdr = null;
            //QuoteService q = null;

            //see if we have a quote from the id already
            int qid;
            if(int.TryParse(v, out qid))
            {
                //
                string query = "SELECT TOP 1 * FROM Quotes WHERE quote_id = '" + qid +"'";
                conn = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=UnReadableCodeDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                cmd = new SqlCommand(query, conn);

                //open DB connection
                conn.Open();


                rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    q.ID = rdr.GetInt32(0);
                    q.item_id = rdr.GetInt32(1);
                    q.date = rdr.GetDateTime(2);
                    q.Cost = rdr.GetDecimal(3);
                }


            }

            //we have to write the query
            string userQuery = "SELECT item_id ";
            userQuery += "FROM ITEMS ";
            userQuery += "WHERE item_name = '" + v + "'";

            conn = new SqlConnection("Data Source=(localdb)\\ProjectsV13;Initial Catalog=UnReadableCodeDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            cmd = new SqlCommand(userQuery, conn);

            //open DB connection
            conn.Open();

            
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                Item i = new Item();
                if(rdr.Read())//only one
                {
                    //now get the value of the id from the row
                    i.item_id = rdr.GetInt32(0); //the id
                }
                rdr.Close();

                //get the quote by calling the web service, right now let's simulate it by using Math.Rand
                c = r.Next(100, 200) + IntAccountID;

                //next we are going to write the quote to the console
                Console.WriteLine(v + " will cost " + c);

                //now assign the quote to the new quote for returning
                q.Cost = r.Next(100, 200);
                q.Item = v;
                q.item_id = i.item_id;


                cmd.CommandText = 
                    "INSERT INTO QUOTES (item_id, Cost, account_id, date) " +
                    "VALUES ('" + q.item_id + "','" + q.Cost + "','" + IntAccountID + "','" + DateTime.Now + "');" +
                    "SELECT @@IDENTITY;";
                q.ID = int.Parse(cmd.ExecuteScalar().ToString());

                
                return q;
            }
            else
            {
                //we didnt find the item
                return null;
            }
        }
    }

    /// <summary>
    /// an item from the database
    /// </summary>
    internal class Item
    {
        /// <summary>
        /// the constructor for an item from the database
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// the id of the item from the database
        /// </summary>
        public int item_id { get; internal set; }
    }
}
