using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercicio_Pablo
{
    class Program
    {
        static void Main(string[] args)
        {
            //user identification.
            Console.WriteLine("*** Is the user an (e)mployee or a (c)ustomer? ***");
            string user = Console.ReadLine();
            Customer.CreateMainFileEmptyAccounts();

            if (user == "c")
            {
                //print login screen for customer.
                Login.LoginCustomer();
            }
            else if (user == "e")
            {

                //print login screen for employee.
                Login.LoginEmployee();
            }
            else
            {
                //prints a message saying that the entered parameter is incorrect as requested in the menu.
                Console.WriteLine("\n\r*** The value entered does not fit the requested menu. ***");
            }
            Console.ReadKey();
        }
    }
    public class Login
    {
        public static bool ConfirmLogin()
        {
            //method to check if the user wants to stay logged in.
            Console.WriteLine("\n\r\n\r*** To stay logged in, type (y).If you want to log out, type any other key. ***");
            string answer = Console.ReadLine();
            if (answer != "y")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void LoginCustomer()
        {
            //assigning customer information. 
            Customer customer = Customer.ChooseCustomerAccount();
            //check if the account entered exists.  
            if (Customer.ExistsAccount(customer))
            {
                Console.WriteLine("\n\r*** Login (Customer) successfully performed!!! ***");
                //calls the client menu while he wants to be logged in.
                bool logged = true;
                while (logged)
                {
                    Menu.MenuCustomer(customer);
                    logged = ConfirmLogin();
                }
            }
            else
            {
                //prints message saying the account entered does not exist.
                Console.WriteLine("\n\r*** No account was found with these credentials. ***");
            }
        }
        public static void LoginEmployee()
        {
            //validate entered password for employee
            Employee employee = new Employee();
            Console.WriteLine("\n\r*** Enter the password used for employee ***");
            string password = Console.ReadLine();
            if (employee.Pin == password)
            {
                bool logged = true;
                //calls the employee menu while he wants to be logged in.
                Console.WriteLine("\n\r*** Login successfully! ***");
                while (logged)
                {
                    Menu.MenuEmployee();
                    logged = ConfirmLogin();
                }
            }
            else
            {
                //if the password is incorrect.
                Console.WriteLine("*** Incorrect password. ***");
            }
        }
    }
    public class File
    {
        public static void DeleteFile(string path)
        {
            //checks if a file exists in the specified path.
            if (System.IO.File.Exists(path))
            {
                //if it exists, delete the file.
                System.IO.File.Delete(path);
            }
        }
        public static void CreateArchive(string path)
        {
            // checks if a file exists in the specified path.
            if (!System.IO.File.Exists(path))
            {
                //if not, creates the blank file.
                System.IO.File.CreateText(path).Close();
            }
        }
        public static List<string[]> SearchFullFile(string path)
        {
            //fetches the entire contents of the file specified by the path parameter
            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            List<string[]> lstResult = new List<string[]>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                //the split() method is used as a line break, where the file is standardized on ; to delimit its columns.
                string[] result = line.Split(';');
                lstResult.Add(result);


            }
            //close the file.
            reader.Close();
            return lstResult;
        }
        public static void RewriteFile(string path, List<string> lstResult)
        {
            //is traversed through the entire list of string, adding it to the file.
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
            foreach (string item in lstResult)
            {
                writer.WriteLine(item);
            }
            writer.Close();
        }
        public static void AddFile(string path, string result)
        {
            //is done adding lines with their content to the file.
            System.IO.StreamWriter writer = System.IO.File.AppendText(path);
            writer.WriteLine(result);
            writer.Close();
        }
        public static string[] SearchLastFileLine(string path)
        {
            //fetches all content of the file specified by the path parameter, but will return only the last record
            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            string[] result = new string[3];
            result[0] = "";
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                //the split() method is used as a line break, where the file is standardized in ; to delimit its columns.
                result = line.Split(';');


            }
            reader.Close();
            return result;
        }
    }
    public class Menu
    {
        public static Transaction ChooseAccountType(Customer customer)
        {
            Transaction transaction = new Transaction();
            //searches all accounts of the specified client.
            List<Transaction> LastTransaction = Transaction.SearchLastAccountTransaction(customer);

            //Lists all the accounts for this customer.
            Transaction.List(LastTransaction);

            //choice of account type.
            Console.WriteLine(string.Format("\n\r*** Which account you want to move: ***" +
                "\n\r{0} - {1} " +
            "\n\r{2} - {3} ", 1,
            (Transaction.eAccountType)1,
            2,
            (Transaction.eAccountType)2
            ));

            int result = int.Parse(Console.ReadLine());

            //for the value entered, the account type is searched through the enum.
            transaction.TypeAccount = ((Transaction.eAccountType)result).ToString();
            transaction.FileName = ((Transaction.eFileName)result).ToString();
            //assigns the current value of the account balance.
            transaction.BalanceValue = LastTransaction.Where(x => x.TypeAccount == transaction.TypeAccount).Select(y => y.BalanceValue).FirstOrDefault();

            transaction.Customer = customer;
            return transaction;
        }
        public static void BeginMenu()
        {
            //begin menu generic
            Console.WriteLine("\n\r*** Choose one of the menu options below: ***");
        }
        public static void MenuEmployee()
        {
            //generic menu initialization method.
            BeginMenu();

            //Employee Menu
            Console.WriteLine("\n\r1 - Create customer " +
          "\n\r2 - Delete Customer " +
          "\n\r3 - List Customer " +
          "\n\r4 - Transaction ");

            int result = int.Parse(Console.ReadLine());
            if (result == 1)
            {
                //account creation.
                Customer.CreateAccount();
            }
            else if (result == 2)
            {
                //deleting account.
                Customer.DeleteAccount();
            }
            else if (result == 3)
            {
                //listing the account.
                List<Customer> lstAllCustomers = Customer.SearchAllAccounts();
                Customer.List(lstAllCustomers);
            }
            else if (result == 4)
            {
                //transaction.
                Customer customer = Customer.ChooseCustomerAccount();
                Transaction.MakeTransaction(customer);
            }
            else
            {
                //prints a message saying that the entered parameter is incorrect as requested in the menu.
                Console.WriteLine("*** The value entered does not fit the requested menu ***");
            }
        }
        public static void MenuCustomer(Customer customer)
        {
            Console.WriteLine("\n\r");
            //menu initialization.
            BeginMenu();
            //Customer menu.
            Console.WriteLine("\n\r1 - Bank Statement " +
                "\n\r2 - Move Money");
            int result = int.Parse(Console.ReadLine());
            if (result == 1)
            {
                //this option will bring only the information to print the bank statement.
                Transaction.BankStatement(customer);
            }
            else if (result == 2)
            {
                //this option will bring only the information to make the transaction.
                Transaction.MakeTransaction(customer);
            }
            else
            {
                //prints a message saying that the entered parameter is incorrect as requested in the menu.
                Console.WriteLine("*** The value entered does not fit the requested menu ***");
            }
        }
    }
    public class Transaction
    {
        public DateTime DataTransaction;
        public string TypeTransaction;
        public string TypeAccount;
        public string FileName;
        public decimal BalanceValue;
        public decimal TransactionValue;
        //enum account type.
        public enum eAccountType
        {
            SAVINGS = 1,
            CURRENT = 2
        };
        //enum transaction type.
        public enum eTransactionType
        {
            Withdrawal = 1,
            Entry = 2
        };
        //enum file name.
        public enum eFileName
        {
            saving = 1,
            current = 2
        }
        public Customer Customer;
        private static string current_path = @"C:";
        public static void CreateArchivesTransaction(string Account_Number)
        {
            //creates transaction files for an account.
            File.CreateArchive(current_path + Account_Number + string.Format("-{0}.txt", eFileName.saving));
            File.CreateArchive(current_path + Account_Number + string.Format("-{0}.txt", eFileName.current));
        }
        public static List<Transaction> SearchAllTransactions(Transaction transaction)
        {

            List<Transaction> lstTransaction = new List<Transaction>();
            //Search all transactions from a specific account.
            foreach (string[] item in File.SearchFullFile(current_path + transaction.Customer.AccountNumber + string.Format("-{0}.txt", transaction.FileName)))
            {
                Transaction new_transaction = new Transaction();
                new_transaction.DataTransaction = DateTime.Parse(item[0]);
                new_transaction.TypeTransaction = item[1];
                new_transaction.TransactionValue = decimal.Parse(item[2]);
                new_transaction.BalanceValue = decimal.Parse(item[3]);

                lstTransaction.Add(new_transaction);
            }
            return lstTransaction;
        }
        public static List<Transaction> SearchLastAccountTransaction(Customer customer)
        {
            List<Transaction> lastTransaction = new List<Transaction>();
            //Search for the last transaction of each type of account for a specific customer.
            if (customer.AccountNumber != null)
            {
                lastTransaction.Add(SearchLastLineFile(current_path + customer.AccountNumber + string.Format("-{0}.txt", eFileName.current), eAccountType.CURRENT));
                lastTransaction.Add(SearchLastLineFile(current_path + customer.AccountNumber + string.Format("-{0}.txt", eFileName.saving), eAccountType.SAVINGS));
            }
            return lastTransaction;
        }
        public static void PerformTransaction(Transaction transaction)
        {
            //search the current date
            transaction.DataTransaction = DateTime.Now.Date;

            if (transaction.TypeTransaction == eTransactionType.Withdrawal.ToString())
            {
                //subtraction of the value.
                transaction.BalanceValue -= transaction.TransactionValue;
            }
            else
            {
                //the addition of the value
                transaction.BalanceValue += transaction.TransactionValue;
            }
            //adds the transaction to the transaction file.
            File.AddFile(current_path + transaction.Customer.AccountNumber + string.Format("-{0}.txt", transaction.FileName),
                string.Format("{0};{1};{2};{3}", transaction.DataTransaction.ToString("dd/MM/yyyy"),
                transaction.TypeTransaction,
                transaction.TransactionValue,
                transaction.BalanceValue
                )
                );
        }
        public static bool ValidateTransactionWithdrawal(Transaction transaction)
        {
            //validates if there is enough balance to withdraw the requested amount.
            if (transaction.BalanceValue >= transaction.TransactionValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void BankStatement(Customer customer)
        {
            //identifies the account type and populates the current account balance.
            Transaction transaction = Menu.ChooseAccountType(customer);

            Console.WriteLine(string.Format("\n\r*** newBank Statement Account - {0} ***", transaction.TypeAccount));
            //searches all transitions for a specific account.
            foreach (Transaction item in SearchAllTransactions(transaction))
            {
                Console.WriteLine(string.Format("{0};{1};{2};{3}",
                    item.DataTransaction.ToString("dd/MM/yyyy"),
                    item.TypeTransaction,
                    item.TransactionValue,
                    item.BalanceValue));
            }
        }
        public static Transaction SearchLastLineFile(string path, eAccountType AccountType)
        {
            //receives the last line of the file.
            string[] result = File.SearchLastFileLine(path);
            Transaction transaction = new Transaction();
            if (!string.IsNullOrEmpty(result[0]))
            {

                transaction.DataTransaction = DateTime.Parse(result[0]);
                transaction.TypeTransaction = result[1];
                transaction.TransactionValue = decimal.Parse(result[2]);
                transaction.BalanceValue = decimal.Parse(result[3]);
            }
            transaction.TypeAccount = AccountType.ToString();

            return transaction;
        }
        public static void List(List<Transaction> LstTransaction)
        {
            //print the account listing on the screen
            foreach (Transaction item in LstTransaction)
            {
                if (item.TypeTransaction != null)
                {
                    Console.WriteLine(string.Format("{0};{1}",
                        item.TypeAccount,
                        item.BalanceValue));
                }
            }
        }
        public static void MakeTransaction(Customer customer)
        {
            //identifies the account type and fills in the current account balance.
            Transaction transaction = Menu.ChooseAccountType(customer);


            //solicits to the user the transaction to be done.
            Console.WriteLine(string.Format("\n\r*** Choose the type of transaction to be made: ***" +
                "\n\r{0} - {1} " +
                "\n\r{2} - {3} ", 1,
                (eTransactionType)1,
                2,
                (eTransactionType)2));
            int transactionType = int.Parse(Console.ReadLine());

            //Requests the value of the transaction.
            Console.WriteLine("\n\r*** Enter the transaction value: ***");
            decimal transactionValue = decimal.Parse(Console.ReadLine());

            transaction.TypeTransaction = ((eTransactionType)transactionType).ToString();
            transaction.TransactionValue = transactionValue;


            if (transaction.TypeTransaction == eTransactionType.Withdrawal.ToString())
            {
                //if the transaction is a withdrawal transaction.
                //validates whether the transaction can be withdrawn by applying the rule specified in the job document.
                if (ValidateTransactionWithdrawal(transaction))
                {
                    //if the account value is greater than or equal to the withdrawal amount the transaction is made.
                    PerformTransaction(transaction);
                }
                else
                {
                    //If the account balance is not greater than or equal to the withdrawal amount, a message is sent rejecting the transaction.
                    Console.WriteLine(string.Format("The amount {0} is above your account balance {1}. " +
                        "\n\rPlease select another transaction or withdraw a smaller amount.", transaction.TransactionValue, transaction.TypeAccount));
                }
            }
            else
            {
                //if it is deposit transaction, the transaction is done directly.
                PerformTransaction(transaction);
            }
        }
    }
    public class User
    {
        public string Pin;
    }
    public class Employee : User
    {
        public Employee()
        {
            //when instantiating the class, call the constructor to set the source employee's default password in the working document.
            Pin = "A1234";
        }
    }
    public class Customer : User
    {
        public string Name;
        public string Surname;
        public string Email;
        public string AccountNumber;
        public List<Transaction> lstTransaction;
        public static void CreateMainFileEmptyAccounts()
        {
            File.CreateArchive(main_path);
        }
        private static string main_path = @"C:customers.txt";
       
        public static void CreateAccount(Customer customer)
        {
            //Save the customer's account.
            SaveAccount(customer, main_path);
            //Generates transaction files for each account of that customer.
            Transaction.CreateArchivesTransaction(customer.AccountNumber);
        }
        public static void RewriteFileAccounts(List<Customer> lstCustomer)
        {
            //generates a string listing where the accounts file will be mounted.
            List<string> lstResult = new List<string>();
            foreach (Customer item in lstCustomer)
            {
                //adds to a listing the line that will be added to the file.

                lstResult.Add(string.Format("{0};{1};{2};{3};{4}", item.Email,
             item.Name,
             item.Surname,
             item.AccountNumber,
             item.Pin
             ));


            }
            if (lstResult.Count > 1)
            {
                //If has something on the list rewritethe file
                File.RewriteFile(main_path, lstResult);
            }
            else
            {
                //If has nothing to write the file will be deleted and recreated
                File.DeleteFile(main_path);
                File.CreateArchive(main_path);
            }
        }
        public static string[] GenerateAccountNumberAndPin(string name, string surName, string[,] alphabet_Code)
        {
            string[] results = new string[2];
            //searches the first letter of the customer's first and second name.
            // substring() method breaks the string according to passed parameters.
            //trim() method removes the blanks.
            //the tolower() method is set to lowercase
            string begin_account = name.Trim().Substring(0, 1).ToLower() + surName.Trim().Substring(0, 1).ToLower();

            //searches the length of the client's entire name.
            // the replace() method removes the space between one name and another in cases where there is more than one last name.
            //the length attribute is a string attribute where you get the length of the string.
            int size_name = (name + surName.Replace(" ", "")).Trim().Length;

            //searches the value of the first letter of the first and second name.
            //has developed the method ReturnAlphabetLetterValue() where the comparison is done in the array to fetch the numeric value of the letter.
            int first_letter_number = ReturnAlphabetLetterValue(alphabet_Code, name.Trim().Substring(0, 1));
            int second_letter_number = ReturnAlphabetLetterValue(alphabet_Code, surName.Trim().Substring(0, 1));

            //through string.format makes it easy to format according to what is specified in the document for account number.
            results[0] = string.Format("{0}-{1}-{2}-{3}", begin_account, size_name, first_letter_number, second_letter_number);
            results[1] = first_letter_number.ToString() + second_letter_number.ToString();
            return results;
        }
        public static int ReturnAlphabetLetterValue(string[,] AlphabetIndex, string initial_letter)
        {
            int number_letter = 0;
            // is scrolled in the for half the size of the hue with the intent of scrolling through all letter
            for (int i = 0; i < AlphabetIndex.Length / 2; i++)
            {
                //The toupper() method is used to place the letter in capitals so the comparison can be done as it is in the array.
                if (initial_letter.Trim().Substring(0, 1).ToUpper() == AlphabetIndex[i, 0])
                {
                    //If it finds the letter, get the numerical value of the letter.
                    //through the continue statement, the process is finished and the rest of the method continues.
                    number_letter = int.Parse(AlphabetIndex[i, 1]);
                    continue;
                }
            }
            return number_letter;
        }
        public static void SaveAccount(Customer customer, string path)
        {
            //adds the account to the file.
            File.AddFile(path, string.Format("{0};{1};{2};{3};{4}", customer.Email,
                customer.Name,
                customer.Surname,
                customer.AccountNumber,
                customer.Pin
                ));
        }
        public static Customer SearchAllAccountsCustomer(Customer customer)
        {
            //by calling all accounts, a filter is performed.
            //using the lambda expression to compare the accounts that have the same attributes.
            Customer objctCustomer = SearchAllAccounts().Where(x => x.Email == customer.Email && x.Name == customer.Name && x.Surname == customer.Surname
             && x.AccountNumber == customer.AccountNumber && x.Pin == customer.Pin).FirstOrDefault();
            if (objctCustomer == null)
            {
                objctCustomer = new Customer();
            }

            objctCustomer.lstTransaction = new List<Transaction>();
            objctCustomer.lstTransaction = Transaction.SearchLastAccountTransaction(objctCustomer);




            return objctCustomer;
        }
        public static List<Customer> SearchAllAccounts()
        {
            List<Customer> lstCustomers = new List<Customer>();
            //is fetched all accounts, reading all lines from the file
            foreach (string[] item in File.SearchFullFile(main_path))
            {
                Customer customer = new Customer();
                customer.Email = item[0];
                customer.Name = item[1];
                customer.Surname = item[2];
                customer.AccountNumber = item[3];
                customer.Pin = item[4];
                //fills in the customer's list of accounts.
                customer.lstTransaction = new List<Transaction>();
                customer.lstTransaction = Transaction.SearchLastAccountTransaction(customer);
                lstCustomers.Add(customer);
            }
            return lstCustomers;
        }
        public static void List(List<Customer> LastCustomer)
        {
            //print the account listing on the screen.
            foreach (Customer item in LastCustomer)
            {
                foreach (Transaction item_transaction in item.lstTransaction)
                {

                    Console.WriteLine(string.Format("{0};{1} {2};{3};{4};{5};{6}", item.Email,
                        item.Name,
                        item.Surname,
                        item.AccountNumber,
                        item.Pin,
                        item_transaction.TypeAccount,
                        item_transaction.BalanceValue));
                }
            }
        }
        public static List<Customer> RemoveAccount(List<Customer> lstCustomer, Customer customer)
        {
            //creates a list to put the accounts in, minus the one that is equal to the "customer" object which will be the deleted one.
            List<Customer> lstCustomer_Updated = new List<Customer>();
            //executes the entire list of accounts.
            foreach (Customer item in lstCustomer)
            {
                //checks if the account in the list is different from the account specified in the method parameter.
                if (!(item.Email == customer.Email && item.Name == customer.Name && item.Surname == customer.Surname
                    && item.AccountNumber == customer.AccountNumber && item.Pin == customer.Pin))
                {
                    //if diffrent add to the list.
                    lstCustomer_Updated.Add(item);
                }
            }
            return lstCustomer_Updated;
        }
        public static bool ExistsAccount(Customer customer)
        {
            //check if there are any accounts in the same file.
            if (!string.IsNullOrEmpty(SearchAllAccountsCustomer(customer).Name))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static Customer CustomerData()
        {
            //impresses the requirements for informing the customer.
            Customer customer = new Customer();
            Console.WriteLine("\n\r*** Enter the first name: ***");
            customer.Name = Console.ReadLine();
            Console.WriteLine("\n\r*** Enter the Surname: ***");
            customer.Surname = Console.ReadLine();
            Console.WriteLine("\n\r*** Enter the email: ***");
            customer.Email = Console.ReadLine();
            return customer;
        }
        public static void CreateAccount()
        {
            //calls the method where the requirements for creating a customer will be printed to the screen.
            Customer customer = CustomerData();
            //Generates the value of the account number and pin.
            customer.AccountNumber = GenerateAccountNumberAndPin(customer.Name, customer.Surname, CreateAlphabetCode())[0];
            customer.Pin = GenerateAccountNumberAndPin(customer.Name, customer.Surname, CreateAlphabetCode())[1];
            //check if this account exists.
            if (!ExistsAccount(customer))
            {
                //call the method where create the customer's account.
                CreateAccount(customer);
                Console.WriteLine("\r\n*** Account created successfully! ***");
            }
            else
            {
                Console.WriteLine("\r\n*** Already existing account. ***");

            }
        }
        public static void DeleteAccount()
        {
            //calls the method where the requirements to identify the customer and their account.
            Customer customer = ChooseCustomerAccount();
            //checks if the customer's accounts are zeroed.
            //the sum method sums all the specified fields in the list
            if (customer.lstTransaction.Sum(x => x.BalanceValue) != 0)
            {
                //Returns a message that the exclusion was not possible due to some value in the customer's account.
                Console.WriteLine("\n\r*** The account has a balance greater than 0, it will not be possible to delete. ***");
            }
            else
            {
                //if the balance is 0, it can be deleted
                //searches all customer accounts
                //draws the one to be deleted from the list.
                //write the file again with the list from which the account to be deleted was removed
                List<Customer> lstAllCustomers = SearchAllAccounts();
                List<Customer> lstUpdateCustomers = RemoveAccount(lstAllCustomers, customer);
                RewriteFileAccounts(lstUpdateCustomers);
                Console.WriteLine("\n\r*** Accounts deleted successfully! ***");
            }
        }
        public static Customer ChooseCustomerAccount()
        {
            Customer customer = new Customer();
            //Method where the customer's requirements will be identified.
            customer = CustomerData();
            Console.WriteLine("\n\r*** Enter account number ***");
            customer.AccountNumber = Console.ReadLine();
            Console.WriteLine("\n\r*** Enter password ***");
            customer.Pin = Console.ReadLine();
            customer = SearchAllAccountsCustomer(customer);
            return customer;
        }
        public static string[,] CreateAlphabetCode()
        {
            //fills the array specified in the job document.
            string[,] Alphabet_Code = new string[26, 2];
            Alphabet_Code[0, 0] = "A";
            Alphabet_Code[1, 0] = "B";
            Alphabet_Code[2, 0] = "C";
            Alphabet_Code[3, 0] = "D";
            Alphabet_Code[4, 0] = "E";
            Alphabet_Code[5, 0] = "F";
            Alphabet_Code[6, 0] = "G";
            Alphabet_Code[7, 0] = "H";
            Alphabet_Code[8, 0] = "I";
            Alphabet_Code[9, 0] = "J";
            Alphabet_Code[10, 0] = "K";
            Alphabet_Code[11, 0] = "L";
            Alphabet_Code[12, 0] = "M";
            Alphabet_Code[13, 0] = "N";
            Alphabet_Code[14, 0] = "O";
            Alphabet_Code[15, 0] = "P";
            Alphabet_Code[16, 0] = "Q";
            Alphabet_Code[17, 0] = "R";
            Alphabet_Code[18, 0] = "S";
            Alphabet_Code[19, 0] = "T";
            Alphabet_Code[20, 0] = "U";
            Alphabet_Code[21, 0] = "V";
            Alphabet_Code[22, 0] = "W";
            Alphabet_Code[23, 0] = "X";
            Alphabet_Code[24, 0] = "Y";
            Alphabet_Code[25, 0] = "Z";
            Alphabet_Code[0, 1] = "1";
            Alphabet_Code[1, 1] = "2";
            Alphabet_Code[2, 1] = "3";
            Alphabet_Code[3, 1] = "4";
            Alphabet_Code[4, 1] = "5";
            Alphabet_Code[5, 1] = "6";
            Alphabet_Code[6, 1] = "7";
            Alphabet_Code[7, 1] = "8";
            Alphabet_Code[8, 1] = "9";
            Alphabet_Code[9, 1] = "10";
            Alphabet_Code[10, 1] = "11";
            Alphabet_Code[11, 1] = "12";
            Alphabet_Code[12, 1] = "13";
            Alphabet_Code[13, 1] = "14";
            Alphabet_Code[14, 1] = "15";
            Alphabet_Code[15, 1] = "16";
            Alphabet_Code[16, 1] = "17";
            Alphabet_Code[17, 1] = "18";
            Alphabet_Code[18, 1] = "19";
            Alphabet_Code[19, 1] = "20";
            Alphabet_Code[20, 1] = "21";
            Alphabet_Code[21, 1] = "22";
            Alphabet_Code[22, 1] = "23";
            Alphabet_Code[23, 1] = "24";
            Alphabet_Code[24, 1] = "25";
            Alphabet_Code[25, 1] = "26";
            return Alphabet_Code;
        }
    }
}
