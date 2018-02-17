using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem
{
    public partial class Form1 : Form
    {
        //sid14
        public Table _table;

        public Form1()
        {
            InitializeComponent();
        }
        //2. Clearing the Labels Displaying the Player Hands 
        private void ClearHandLabels()
        {
            {
                this.lblHand1.Text = String.Empty;
                this.lblHand2.Text = String.Empty;
                this.lblHand3.Text = String.Empty;
                this.lblHand4.Text = String.Empty;
            }
        }

        //1. Adding player Names
        private void btnNewTable_Click(object sender, EventArgs e)
        {
            try
            {
                lblHand2.Text = "nuevo";
                List<string> names = new List<string>(); //sparar en lista av spelare
                if (txtPlayerName1.Text.Length > 0)
                {
                    names.Add(txtPlayerName1.Text);
                    lblPlayerName1.Text = names.ElementAt(0); //Sparar namnet i en ARRAY
                }
                if (txtPlayerName2.Text.Length > 0)
                {
                    names.Add(txtPlayerName2.Text);
                    lblPlayerName2.Text = names.ElementAt(1);
                }
                if (txtPlayerName3.Text.Length > 0)
                {
                    names.Add(txtPlayerName3.Text);
                    lblPlayerName3.Text = names.ElementAt(2);
                }
                if (txtPlayerName4.Text.Length > 0)
                {
                    names.Add(txtPlayerName4.Text);
                    lblPlayerName4.Text = names.ElementAt(3); ;
                }

                else
                {
                    MessageBox.Show("The textbox is empty!");
                    return;
                }
                ClearHandLabels();
                ClearPlayerCardsFromTable();
                ClearDealerCardsFromTable();

                //sid14
                _table = new Table(names.ToArray());
                //Player player = new Player("kalle");
                //player.ReceiveCard(new Card(Values.Jack, Suits.Hearts), true);

                //3. Enabling / Disabling Buttons and Labels
                this.btnDrawCard.Enabled = false;
                this.btnNewHand.Enabled = true;
                this.lblWinner.Hide();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {

            }
        }

        //6. Displaying Cards on the Table
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //var card = new Card(Values.Jack, Suits.Clubs); //_playerCardLabels.Add(new Card(Values.King, Suits.Hearts));
                //Omvandla Listan till Array: 8 _playerCardLabels, å 5 _dealerCardLabels kort.
                //_playerCardLabels.Add(CreateCard(_playerLblPos[0].X, _playerLblPos[0].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[1].X, _playerLblPos[1].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[2].X, _playerLblPos[2].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[3].X, _playerLblPos[3].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[4].X, _playerLblPos[4].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[5].X, _playerLblPos[5].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[6].X, _playerLblPos[6].Y, card));
                //_playerCardLabels.Add(CreateCard(_playerLblPos[7].X, _playerLblPos[7].Y, card));
                //Controls.AddRange(_playerCardLabels.ToArray());

                //_dealerCardLabels.Add(CreateCard(_dealerLblPos[0].X, _dealerLblPos[0].Y, card));
                //_dealerCardLabels.Add(CreateCard(_dealerLblPos[1].X, _dealerLblPos[1].Y, card));
                //_dealerCardLabels.Add(CreateCard(_dealerLblPos[2].X, _dealerLblPos[2].Y, card));
                //_dealerCardLabels.Add(CreateCard(_dealerLblPos[3].X, _dealerLblPos[3].Y, card));
                //_dealerCardLabels.Add(CreateCard(_dealerLblPos[4].X, _dealerLblPos[4].Y, card));
                //Controls.AddRange(_dealerCardLabels.ToArray());

                //Instancen Deck:
                //var deck = new Deck();
                //deck.ShuffleDeck(7);
                //var card = deck.DrawCard(); //Returnerar ett kort

            } 

            catch (ArgumentException ex)
            {

            }
        }
              //8. Clearing the Cards from the Table
        private void ClearPlayerCardsFromTable()
        {
            foreach (var playerCardLabel in _playerCardLabels)
            {
                Controls.Remove(playerCardLabel);
            }
            _playerCardLabels.Clear();
        }
        private void ClearDealerCardsFromTable()
        {
            foreach (var dealerCardLabel in _dealerCardLabels)
            {
                Controls.Remove(dealerCardLabel);
            }
            _dealerCardLabels.Clear();
           
           //Call methods:
            ClearHandLabels();
            ClearPlayerCardsFromTable();
        }

        //7. Creating the Method That Displays a Card on the Table
        private List<Label> _playerCardLabels = new List<Label>();
        private List<Label> _dealerCardLabels = new List<Label>();

        private Point[] _playerLblPos = new Point[]
            {
                new Point(630, 265), new Point(680, 265),
                new Point(500, 365), new Point(550, 365),
                new Point(300, 365), new Point(350, 365),
                new Point(160, 265), new Point(210, 265),
            };
        private Point[] _dealerLblPos = new Point[]
            {
                new Point(320, 265), new Point(370, 265),
                new Point(420, 265), new Point(470, 265),
                new Point(520, 265) 
            };

        //The method:
        private Label CreateCard(int x, int y, Card card)
        {
            Label lbl = new Label();
            lbl.Text = card.Output; //Format the label text value and symbol on separate rows
            lbl.Size = new Size(45, 60);
            lbl.Location = new Point(x, y);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Font = new Font("Consolas", 15);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.BackColor = Color.White;
            lbl.ForeColor = card.Suit.Equals(Suits.Hearts) || card.Suit.Equals(Suits.Diamonds) ? Color.Red : Color.Black;
            return lbl;
        }

        //13. Displaying the Players Cards on the Table:
        private void DisplayPlayerHands()
        {
            var counter = 0;
            foreach (var players in _table.Players)
            {
                var cards = players.PlayerCards;
                _playerCardLabels.Add(CreateCard(_playerLblPos[counter].X, _playerLblPos[counter].Y, new Card(cards[0].Value, cards[0].Suit)));
                _playerCardLabels.Add(CreateCard(_playerLblPos[counter + 1].X, _playerLblPos[counter + 1].Y, new Card(cards[1].Value, cards[1].Suit)));
                counter += 2;
            }
            Controls.AddRange(_playerCardLabels.ToArray());
        }

        private void btnNewHand_Click(object sender, EventArgs e)
        {
            if (_table == null) return;

            _table.DealNewHand();
            ClearDealerCardsFromTable();
            DisplayPlayerHands();
            this.btnDrawCard.Enabled = true;
            this.lblWinner.Hide();

            //ClearDealerCardsFromTable();
            _table.EvaluatePlayerHands();
            FillHandValueLabels();
        }
        
        private void lblPlayerName1_Click(object sender, EventArgs e)
        {

        }
        private void lblHand1_Click(object sender, EventArgs e)
        {

        }
       
        //15. Displaying the Dealer’s Cards on the Table
        private void btnDrawCard_Click(object sender, EventArgs e)
        {
            if (_table.Dealer.CardCount >= 3)
            {
                _table.DealerDrawsCard();
            }
            else 
            {
                _table.DealerDrawsCard(3);
            }
                DisplayDealerCards();
            _table.EvaluatePlayerHands();
            FillHandValueLabels();
            if (_table.Dealer.CardCount == 5)
            {
                DisplayWinner();
            }
        }

        //Sid 19:
        private void FillHandValueLabels()
        {
            lstPlayers.Items.Clear();
            foreach (var player in _table.Players)
            {
                lstPlayers.Items.Add($"{player.Name} ({player.HandValue})"); //listan: resultat
            }
        }
        //Sid 20: Winner:
        private void DisplayWinner()
        {
            lstScores.Items.Clear();
            var winner = _table.DetermineWinner();
            foreach (var determineWinner in winner)
            {
                lstScores.Items.Add($"{determineWinner.Name} ({determineWinner.HandValue})"); //Winnarens lista
            }
        }

        private void DisplayDealerCards()
        {
            var counter = 0;
            foreach (var card in _table.Dealer.Cards)
            {
                _dealerCardLabels.Add(CreateCard(_dealerLblPos[counter].X, _dealerLblPos[counter].Y, new Card(card.Value, card.Suit)));
                counter ++;
            }
            Controls.AddRange(_dealerCardLabels.ToArray());

            if (_table.Dealer.CardCount >= 5)
            {
                btnDrawCard.Enabled = false;
            }
        }
    }
}
