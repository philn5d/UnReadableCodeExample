using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReadableCodeServices;

namespace QuotingWindowsApplication
{
    public partial class QuotingForm : Form
    {
        IQuotingService _quotingService;
        public QuotingForm(string something, IQuotingService quotingService)
        {
            InitializeComponent();
            Text = something;
            _quotingService = quotingService;
        }

        private void GetQuoteButton_Click(object sender, EventArgs e)
        {
            var quote = _quotingService.GetQuote(QuoteParameters);
            DisplayQuoteDetails(quote);
        }

        private void DisplayQuoteDetails(decimal quote)
        {
            QuoteDetails.Text = quote.ToString();
        }

        private GetQuoteParameters QuoteParameters
        {
            get
            {
                return new GetQuoteParameters
                {
                    AccountId = GetIntFromTextBox(AccountId),
                    ItemIds = new[] { GetIntFromTextBox(ItemId) }
                };
            }
        }
        
        private int GetIntFromTextBox(TextBox textBox)
        {
            return int.Parse(textBox.Text);
        }
    }
}
