using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using PPGSage50Plugin.Models;
using PPGSage50Plugin.Services;

namespace PPGSage50Plugin.UI
{
    /// <summary>
    /// Formulaire de création et gestion des commandes
    /// </summary>
    public partial class OrderForm : Form
    {
        private readonly PPGSage50Plugin _plugin;
        private Order _currentOrder;
        private List<OrderLine> _orderLines;

        // Contrôles principaux
        private TabControl _mainTabControl;
        private TabPage _orderTab;
        private TabPage _linesTab;
        private TabPage _simulationTab;

        // Onglet Commande
        private ComboBox _customerComboBox;
        private DateTimePicker _orderDatePicker;
        private DateTimePicker _deliveryDatePicker;
        private TextBox _notesTextBox;
        private TextBox _deliveryAddressTextBox;

        // Onglet Lignes
        private DataGridView _linesDataGridView;
        private Button _addLineButton;
        private Button _removeLineButton;

        // Onglet Simulation
        private TextBox _simulationTextBox;
        private Button _simulateButton;
        private Button _sendButton;

        public OrderForm(PPGSage50Plugin plugin, Order order = null)
        {
            _plugin = plugin;
            _currentOrder = order ?? new Order();
            _orderLines = new List<OrderLine>(_currentOrder.Lines);
            
            InitializeComponent();
            InitializeUI();
            LoadOrderData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuration du formulaire
            this.Text = _currentOrder.Id == null ? "Nouvelle commande" : $"Commande {_currentOrder.OrderNumber}";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(800, 600);
            this.Icon = SystemIcons.Application;
            
            this.ResumeLayout(false);
        }

        private void InitializeUI()
        {
            // Créer le contrôle principal
            _mainTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F)
            };

            // Créer les onglets
            CreateOrderTab();
            CreateLinesTab();
            CreateSimulationTab();

            // Ajouter les onglets au contrôle principal
            _mainTabControl.TabPages.AddRange(new TabPage[]
            {
                _orderTab,
                _linesTab,
                _simulationTab
            });

            // Créer la barre d'outils
            CreateToolStrip();

            // Ajouter les contrôles au formulaire
            this.Controls.Add(_mainTabControl);
        }

        private void CreateOrderTab()
        {
            _orderTab = new TabPage("Informations commande");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6
            };

            // Colonnes
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Client
            panel.Controls.Add(new Label { Text = "Client:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            _customerComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };
            _customerComboBox.SelectedIndexChanged += CustomerComboBox_SelectedIndexChanged;
            panel.Controls.Add(_customerComboBox, 1, 0);

            // Date commande
            panel.Controls.Add(new Label { Text = "Date commande:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            _orderDatePicker = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(_orderDatePicker, 1, 1);

            // Date livraison
            panel.Controls.Add(new Label { Text = "Date livraison:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
            _deliveryDatePicker = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(_deliveryDatePicker, 1, 2);

            // Adresse de livraison
            panel.Controls.Add(new Label { Text = "Adresse livraison:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 3);
            _deliveryAddressTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };
            panel.Controls.Add(_deliveryAddressTextBox, 1, 3);

            // Notes
            panel.Controls.Add(new Label { Text = "Notes:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft }, 0, 4);
            _notesTextBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ScrollBars = ScrollBars.Vertical
            };
            panel.Controls.Add(_notesTextBox, 1, 4);

            _orderTab.Controls.Add(panel);
        }

        private void CreateLinesTab()
        {
            _linesTab = new TabPage("Lignes de commande");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Panel d'outils
            var toolsPanel = new Panel { Dock = DockStyle.Fill };
            _addLineButton = new Button
            {
                Text = "Ajouter ligne",
                Size = new Size(120, 30),
                Location = new Point(10, 10)
            };
            _addLineButton.Click += AddLineButton_Click;

            _removeLineButton = new Button
            {
                Text = "Supprimer ligne",
                Size = new Size(120, 30),
                Location = new Point(140, 10),
                Enabled = false
            };
            _removeLineButton.Click += RemoveLineButton_Click;

            toolsPanel.Controls.AddRange(new Control[] { _addLineButton, _removeLineButton });

            // Grille des lignes
            _linesDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9F)
            };
            _linesDataGridView.SelectionChanged += LinesDataGridView_SelectionChanged;

            // Configurer les colonnes
            SetupLinesDataGridView();

            // Panel de statut
            var statusPanel = new Panel { Dock = DockStyle.Fill };
            var totalLabel = new Label
            {
                Text = "Total: 0,00 €",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            statusPanel.Controls.Add(totalLabel);

            // Ajouter les contrôles au panel
            panel.Controls.Add(toolsPanel, 0, 0);
            panel.Controls.Add(_linesDataGridView, 0, 1);
            panel.Controls.Add(statusPanel, 0, 2);

            _linesTab.Controls.Add(panel);
        }

        private void CreateSimulationTab()
        {
            _simulationTab = new TabPage("Simulation & Envoi");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Panel de contrôles
            var controlsPanel = new Panel { Dock = DockStyle.Fill };
            _simulateButton = new Button
            {
                Text = "Simuler commande",
                Size = new Size(150, 30),
                Location = new Point(10, 10)
            };
            _simulateButton.Click += async (s, e) => await SimulateOrderAsync();

            _sendButton = new Button
            {
                Text = "Envoyer commande",
                Size = new Size(150, 30),
                Location = new Point(170, 10),
                Enabled = false
            };
            _sendButton.Click += async (s, e) => await SendOrderAsync();

            controlsPanel.Controls.AddRange(new Control[] { _simulateButton, _sendButton });

            // Zone de simulation
            _simulationTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                ScrollBars = ScrollBars.Vertical
            };

            // Panel de statut
            var statusPanel = new Panel { Dock = DockStyle.Fill };
            var statusLabel = new Label
            {
                Text = "Prêt pour simulation",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            statusPanel.Controls.Add(statusLabel);

            // Ajouter les contrôles au panel
            panel.Controls.Add(controlsPanel, 0, 0);
            panel.Controls.Add(_simulationTextBox, 0, 1);
            panel.Controls.Add(statusPanel, 0, 2);

            _simulationTab.Controls.Add(panel);
        }

        private void CreateToolStrip()
        {
            var toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top
            };

            var saveButton = new ToolStripButton("Sauvegarder", null, SaveButton_Click);
            var cancelButton = new ToolStripButton("Annuler", null, CancelButton_Click);

            toolStrip.Items.Add(saveButton);
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(cancelButton);

            this.Controls.Add(toolStrip);
        }

        private void SetupLinesDataGridView()
        {
            _linesDataGridView.Columns.Clear();

            // Colonne Produit
            var productColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Produit",
                Name = "ProductCode",
                Width = 150
            };
            _linesDataGridView.Columns.Add(productColumn);

            // Colonne Description
            var descriptionColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Description",
                Name = "Description",
                ReadOnly = true,
                Width = 200
            };
            _linesDataGridView.Columns.Add(descriptionColumn);

            // Colonne Quantité
            var quantityColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantité",
                Name = "Quantity",
                Width = 80
            };
            _linesDataGridView.Columns.Add(quantityColumn);

            // Colonne Prix unitaire
            var unitPriceColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Prix unitaire",
                Name = "UnitPrice",
                Width = 100
            };
            _linesDataGridView.Columns.Add(unitPriceColumn);

            // Colonne Remise
            var discountColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Remise %",
                Name = "DiscountPercentage",
                Width = 80
            };
            _linesDataGridView.Columns.Add(discountColumn);

            // Colonne Total ligne
            var lineTotalColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Total ligne",
                Name = "LineTotal",
                ReadOnly = true,
                Width = 100
            };
            _linesDataGridView.Columns.Add(lineTotalColumn);

            // Événements
            _linesDataGridView.CellValueChanged += LinesDataGridView_CellValueChanged;
            _linesDataGridView.CellEndEdit += LinesDataGridView_CellEndEdit;
        }

        private void LoadOrderData()
        {
            // Charger les clients
            LoadCustomers();

            // Charger les données de la commande
            if (_currentOrder.Id != null)
            {
                _customerComboBox.SelectedValue = _currentOrder.CustomerId;
                _orderDatePicker.Value = _currentOrder.OrderDate;
                _deliveryDatePicker.Value = _currentOrder.DeliveryDate ?? DateTime.Today.AddDays(7);
                _notesTextBox.Text = _currentOrder.Notes;
                _deliveryAddressTextBox.Text = _currentOrder.DeliveryAddress;
            }
            else
            {
                _orderDatePicker.Value = DateTime.Today;
                _deliveryDatePicker.Value = DateTime.Today.AddDays(7);
            }

            // Charger les lignes
            LoadOrderLines();
        }

        private async void LoadCustomers()
        {
            try
            {
                var criteria = new CustomerSearchCriteria
                {
                    IsActive = true,
                    PageSize = 1000
                };
                
                var customers = await _plugin.SearchCustomersAsync(criteria);
                
                _customerComboBox.DataSource = customers;
                _customerComboBox.DisplayMember = "Name";
                _customerComboBox.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des clients: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderLines()
        {
            _linesDataGridView.Rows.Clear();
            
            foreach (var line in _orderLines)
            {
                var row = new DataGridViewRow();
                row.CreateCells(_linesDataGridView);
                
                row.Cells["ProductCode"].Value = line.ProductCode;
                row.Cells["Description"].Value = line.ProductName;
                row.Cells["Quantity"].Value = line.Quantity;
                row.Cells["UnitPrice"].Value = line.UnitPrice;
                row.Cells["DiscountPercentage"].Value = line.DiscountPercentage;
                row.Cells["LineTotal"].Value = line.LineTotal;
                
                _linesDataGridView.Rows.Add(row);
            }
            
            UpdateTotal();
        }

        private void CustomerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_customerComboBox.SelectedItem is Customer customer)
            {
                _currentOrder.CustomerId = customer.Id;
                _currentOrder.CustomerCode = customer.Code;
                
                // Charger l'adresse de livraison par défaut
                if (string.IsNullOrEmpty(_deliveryAddressTextBox.Text))
                {
                    _deliveryAddressTextBox.Text = customer.Address;
                }
            }
        }

        private void AddLineButton_Click(object sender, EventArgs e)
        {
            var newLine = new OrderLine
            {
                Id = Guid.NewGuid().ToString(),
                ProductCode = "",
                ProductName = "",
                Quantity = 1,
                UnitPrice = 0,
                DiscountPercentage = 0,
                LineTotal = 0
            };
            
            _orderLines.Add(newLine);
            LoadOrderLines();
        }

        private void RemoveLineButton_Click(object sender, EventArgs e)
        {
            if (_linesDataGridView.SelectedRows.Count > 0)
            {
                var selectedIndex = _linesDataGridView.SelectedRows[0].Index;
                _orderLines.RemoveAt(selectedIndex);
                LoadOrderLines();
            }
        }

        private void LinesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            _removeLineButton.Enabled = _linesDataGridView.SelectedRows.Count > 0;
        }

        private void LinesDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _orderLines.Count)
            {
                var row = _linesDataGridView.Rows[e.RowIndex];
                var line = _orderLines[e.RowIndex];
                
                switch (e.ColumnIndex)
                {
                    case 0: // ProductCode
                        line.ProductCode = row.Cells["ProductCode"].Value?.ToString() ?? "";
                        // Charger les détails du produit
                        LoadProductDetails(line, row);
                        break;
                    case 2: // Quantity
                        if (decimal.TryParse(row.Cells["Quantity"].Value?.ToString(), out decimal quantity))
                        {
                            line.Quantity = quantity;
                            CalculateLineTotal(line, row);
                        }
                        break;
                    case 3: // UnitPrice
                        if (decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out decimal unitPrice))
                        {
                            line.UnitPrice = unitPrice;
                            CalculateLineTotal(line, row);
                        }
                        break;
                    case 4: // DiscountPercentage
                        if (decimal.TryParse(row.Cells["DiscountPercentage"].Value?.ToString(), out decimal discount))
                        {
                            line.DiscountPercentage = discount;
                            CalculateLineTotal(line, row);
                        }
                        break;
                }
            }
        }

        private void LinesDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotal();
        }

        private async void LoadProductDetails(OrderLine line, DataGridViewRow row)
        {
            if (!string.IsNullOrEmpty(line.ProductCode))
            {
                try
                {
                    var product = await _productService.GetProductDetailsAsync(line.ProductCode);
                    if (product.Success)
                    {
                        line.ProductName = product.Data.Name;
                        line.UnitPrice = product.Data.UnitPrice;
                        
                        row.Cells["Description"].Value = product.Data.Name;
                        row.Cells["UnitPrice"].Value = product.Data.UnitPrice;
                        
                        CalculateLineTotal(line, row);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement du produit: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void CalculateLineTotal(OrderLine line, DataGridViewRow row)
        {
            var subtotal = line.Quantity * line.UnitPrice;
            var discountAmount = subtotal * (line.DiscountPercentage / 100);
            line.LineTotal = subtotal - discountAmount;
            
            row.Cells["LineTotal"].Value = line.LineTotal.ToString("F2");
        }

        private void UpdateTotal()
        {
            var total = _orderLines.Sum(line => line.LineTotal);
            var totalLabel = GetControlInTab<Label>(_linesTab, 2);
            if (totalLabel != null)
            {
                totalLabel.Text = $"Total: {total:F2} €";
            }
        }

        private async Task SimulateOrderAsync()
        {
            try
            {
                _simulateButton.Enabled = false;
                _simulationTextBox.Text = "Simulation en cours...\n";
                
                // Mettre à jour la commande avec les données du formulaire
                UpdateOrderFromForm();
                
                var simulation = await _plugin.SimulateOrderAsync(_currentOrder);
                
                _simulationTextBox.Text = $"=== RÉSULTAT DE LA SIMULATION ===\n\n";
                _simulationTextBox.Text += $"Statut: {(simulation.IsValid ? "VALIDÉE" : "ERREUR")}\n";
                _simulationTextBox.Text += $"Message: {simulation.Message}\n\n";
                
                if (simulation.IsValid)
                {
                    _simulationTextBox.Text += $"Sous-total: {simulation.SubTotal:F2} {simulation.Currency}\n";
                    _simulationTextBox.Text += $"Montant TVA: {simulation.TaxAmount:F2} {simulation.Currency}\n";
                    _simulationTextBox.Text += $"TOTAL: {simulation.TotalAmount:F2} {simulation.Currency}\n\n";
                    
                    if (simulation.Warnings.Count > 0)
                    {
                        _simulationTextBox.Text += "AVERTISSEMENTS:\n";
                        foreach (var warning in simulation.Warnings)
                        {
                            _simulationTextBox.Text += $"- {warning}\n";
                        }
                        _simulationTextBox.Text += "\n";
                    }
                    
                    _sendButton.Enabled = true;
                }
                else
                {
                    _simulationTextBox.Text += "ERREURS:\n";
                    foreach (var error in simulation.Errors)
                    {
                        _simulationTextBox.Text += $"- {error}\n";
                    }
                    _sendButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                _simulationTextBox.Text += $"ERREUR: {ex.Message}\n";
                _sendButton.Enabled = false;
            }
            finally
            {
                _simulateButton.Enabled = true;
            }
        }

        private async Task SendOrderAsync()
        {
            try
            {
                var result = MessageBox.Show("Êtes-vous sûr de vouloir envoyer cette commande à PPG Live ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _sendButton.Enabled = false;
                    _simulationTextBox.Text += "\n=== ENVOI DE LA COMMANDE ===\n";
                    _simulationTextBox.Text += "Envoi en cours...\n";
                    
                    // Mettre à jour la commande avec les données du formulaire
                    UpdateOrderFromForm();
                    
                    var order = await _plugin.SendOrderV2Async(_currentOrder);
                    
                    _simulationTextBox.Text += $"Commande envoyée avec succès!\n";
                    _simulationTextBox.Text += $"Numéro de commande PPG Live: {order.OrderNumber}\n";
                    _simulationTextBox.Text += $"ID: {order.Id}\n";
                    
                    MessageBox.Show($"Commande envoyée avec succès!\nNuméro: {order.OrderNumber}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                _simulationTextBox.Text += $"ERREUR: {ex.Message}\n";
                MessageBox.Show($"Erreur lors de l'envoi de la commande: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _sendButton.Enabled = true;
            }
        }

        private void UpdateOrderFromForm()
        {
            _currentOrder.OrderDate = _orderDatePicker.Value;
            _currentOrder.DeliveryDate = _deliveryDatePicker.Value;
            _currentOrder.Notes = _notesTextBox.Text;
            _currentOrder.DeliveryAddress = _deliveryAddressTextBox.Text;
            _currentOrder.Lines = new List<OrderLine>(_orderLines);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            UpdateOrderFromForm();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private T GetControlInTab<T>(TabPage tab, int index) where T : Control
        {
            var tableLayout = tab.Controls[0] as TableLayoutPanel;
            if (tableLayout != null)
            {
                return tableLayout.Controls[index] as T;
            }
            return null;
        }
    }
}
