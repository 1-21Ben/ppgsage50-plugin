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
    /// Formulaire principal du plugin PPG Sage 50
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly PPGSage50Plugin _plugin;
        private TabControl _mainTabControl;
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;
        private ToolStripProgressBar _progressBar;

        // Onglets
        private TabPage _customersTab;
        private TabPage _productsTab;
        private TabPage _ordersTab;
        private TabPage _invoicesTab;
        private TabPage _quotationsTab;
        private TabPage _syncTab;
        private TabPage _settingsTab;

        public MainForm()
        {
            InitializeComponent();
            _plugin = new PPGSage50Plugin();
            InitializeUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuration du formulaire principal
            this.Text = "Plugin PPG Sage 50 - Intégration PPG Live";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
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
            CreateCustomersTab();
            CreateProductsTab();
            CreateOrdersTab();
            CreateInvoicesTab();
            CreateQuotationsTab();
            CreateSyncTab();
            CreateSettingsTab();

            // Ajouter les onglets au contrôle principal
            _mainTabControl.TabPages.AddRange(new TabPage[]
            {
                _customersTab,
                _productsTab,
                _ordersTab,
                _invoicesTab,
                _quotationsTab,
                _syncTab,
                _settingsTab
            });

            // Créer la barre de statut
            CreateStatusStrip();

            // Ajouter les contrôles au formulaire
            this.Controls.Add(_mainTabControl);
            this.Controls.Add(_statusStrip);

            // Charger les données initiales
            LoadInitialData();
        }

        private void CreateCustomersTab()
        {
            _customersTab = new TabPage("Clients");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3
            };

            // Colonnes
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Panel de recherche
            var searchPanel = new Panel { Dock = DockStyle.Fill };
            var searchTextBox = new TextBox
            {
                PlaceholderText = "Rechercher un client...",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };
            var searchButton = new Button
            {
                Text = "Rechercher",
                Dock = DockStyle.Right,
                Width = 100
            };
            searchButton.Click += async (s, e) => await SearchCustomersAsync(searchTextBox.Text);
            
            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(searchButton);

            // Liste des clients
            var customersListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F)
            };

            // Détails du client
            var detailsPanel = new Panel { Dock = DockStyle.Fill };
            var detailsTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                ScrollBars = ScrollBars.Vertical
            };

            detailsPanel.Controls.Add(detailsTextBox);

            // Boutons d'action
            var actionPanel = new Panel { Dock = DockStyle.Fill };
            var syncCustomersButton = new Button
            {
                Text = "Synchroniser les clients",
                Dock = DockStyle.Left,
                Width = 150
            };
            syncCustomersButton.Click += async (s, e) => await SyncCustomersAsync();

            actionPanel.Controls.Add(syncCustomersButton);

            // Ajouter les contrôles au panel
            panel.Controls.Add(searchPanel, 0, 0);
            panel.Controls.Add(customersListBox, 0, 1);
            panel.Controls.Add(detailsPanel, 1, 1);
            panel.Controls.Add(actionPanel, 0, 2);

            _customersTab.Controls.Add(panel);
        }

        private void CreateProductsTab()
        {
            _productsTab = new TabPage("Produits & Stock");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3
            };

            // Colonnes
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Panel de recherche
            var searchPanel = new Panel { Dock = DockStyle.Fill };
            var searchTextBox = new TextBox
            {
                PlaceholderText = "Rechercher un produit...",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };
            var searchButton = new Button
            {
                Text = "Rechercher",
                Dock = DockStyle.Right,
                Width = 100
            };
            searchButton.Click += async (s, e) => await SearchProductsAsync(searchTextBox.Text);

            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(searchButton);

            // Liste des produits
            var productsListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F)
            };

            // Détails du produit
            var detailsPanel = new Panel { Dock = DockStyle.Fill };
            var detailsTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                ScrollBars = ScrollBars.Vertical
            };

            detailsPanel.Controls.Add(detailsTextBox);

            // Boutons d'action
            var actionPanel = new Panel { Dock = DockStyle.Fill };
            var syncProductsButton = new Button
            {
                Text = "Synchroniser les produits",
                Dock = DockStyle.Left,
                Width = 150
            };
            syncProductsButton.Click += async (s, e) => await SyncProductsAsync();

            actionPanel.Controls.Add(syncProductsButton);

            // Ajouter les contrôles au panel
            panel.Controls.Add(searchPanel, 0, 0);
            panel.Controls.Add(productsListBox, 0, 1);
            panel.Controls.Add(detailsPanel, 1, 1);
            panel.Controls.Add(actionPanel, 0, 2);

            _productsTab.Controls.Add(panel);
        }

        private void CreateOrdersTab()
        {
            _ordersTab = new TabPage("Commandes");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Panel de filtres
            var filtersPanel = new Panel { Dock = DockStyle.Fill };
            var customerComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Dock = DockStyle.Left
            };
            var statusComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Dock = DockStyle.Left
            };
            var refreshButton = new Button
            {
                Text = "Actualiser",
                Width = 100,
                Dock = DockStyle.Right
            };
            refreshButton.Click += async (s, e) => await LoadOrdersAsync();

            filtersPanel.Controls.Add(refreshButton);
            filtersPanel.Controls.Add(statusComboBox);
            filtersPanel.Controls.Add(customerComboBox);

            // Liste des commandes
            var ordersDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Panel de détails
            var detailsPanel = new Panel { Dock = DockStyle.Fill };
            var detailsTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                ScrollBars = ScrollBars.Vertical
            };

            detailsPanel.Controls.Add(detailsTextBox);

            // Boutons d'action
            var actionPanel = new Panel { Dock = DockStyle.Fill };
            var simulateButton = new Button
            {
                Text = "Simuler",
                Width = 100,
                Dock = DockStyle.Left
            };
            var sendButton = new Button
            {
                Text = "Envoyer",
                Width = 100,
                Dock = DockStyle.Left
            };

            actionPanel.Controls.Add(sendButton);
            actionPanel.Controls.Add(simulateButton);

            // Ajouter les contrôles au panel
            panel.Controls.Add(filtersPanel, 0, 0);
            panel.Controls.Add(ordersDataGridView, 0, 1);
            panel.Controls.Add(detailsPanel, 0, 2);
            panel.Controls.Add(actionPanel, 0, 3);

            _ordersTab.Controls.Add(panel);
        }

        private void CreateInvoicesTab()
        {
            _invoicesTab = new TabPage("Factures");
            
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

            // Panel de filtres
            var filtersPanel = new Panel { Dock = DockStyle.Fill };
            var customerComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Dock = DockStyle.Left
            };
            var refreshButton = new Button
            {
                Text = "Actualiser",
                Width = 100,
                Dock = DockStyle.Right
            };
            refreshButton.Click += async (s, e) => await LoadInvoicesAsync();

            filtersPanel.Controls.Add(refreshButton);
            filtersPanel.Controls.Add(customerComboBox);

            // Liste des factures
            var invoicesDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Boutons d'action
            var actionPanel = new Panel { Dock = DockStyle.Fill };
            var downloadPdfButton = new Button
            {
                Text = "Télécharger PDF",
                Width = 120,
                Dock = DockStyle.Left
            };

            actionPanel.Controls.Add(downloadPdfButton);

            // Ajouter les contrôles au panel
            panel.Controls.Add(filtersPanel, 0, 0);
            panel.Controls.Add(invoicesDataGridView, 0, 1);
            panel.Controls.Add(actionPanel, 0, 2);

            _invoicesTab.Controls.Add(panel);
        }

        private void CreateQuotationsTab()
        {
            _quotationsTab = new TabPage("Devis");
            
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

            // Panel de filtres
            var filtersPanel = new Panel { Dock = DockStyle.Fill };
            var customerComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Dock = DockStyle.Left
            };
            var refreshButton = new Button
            {
                Text = "Actualiser",
                Width = 100,
                Dock = DockStyle.Right
            };
            refreshButton.Click += async (s, e) => await LoadQuotationsAsync();

            filtersPanel.Controls.Add(refreshButton);
            filtersPanel.Controls.Add(customerComboBox);

            // Liste des devis
            var quotationsDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Boutons d'action
            var actionPanel = new Panel { Dock = DockStyle.Fill };
            var createButton = new Button
            {
                Text = "Créer devis",
                Width = 100,
                Dock = DockStyle.Left
            };
            var convertButton = new Button
            {
                Text = "Convertir en commande",
                Width = 150,
                Dock = DockStyle.Left
            };

            actionPanel.Controls.Add(convertButton);
            actionPanel.Controls.Add(createButton);

            // Ajouter les contrôles au panel
            panel.Controls.Add(filtersPanel, 0, 0);
            panel.Controls.Add(quotationsDataGridView, 0, 1);
            panel.Controls.Add(actionPanel, 0, 2);

            _quotationsTab.Controls.Add(panel);
        }

        private void CreateSyncTab()
        {
            _syncTab = new TabPage("Synchronisation");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            // Panel de contrôles
            var controlsPanel = new Panel { Dock = DockStyle.Fill };
            var syncAllButton = new Button
            {
                Text = "Synchronisation complète",
                Size = new Size(200, 40),
                Location = new Point(10, 10)
            };
            syncAllButton.Click += async (s, e) => await SyncAllAsync();

            var syncCustomersButton = new Button
            {
                Text = "Synchroniser clients",
                Size = new Size(150, 30),
                Location = new Point(10, 60)
            };
            syncCustomersButton.Click += async (s, e) => await SyncCustomersAsync();

            var syncProductsButton = new Button
            {
                Text = "Synchroniser produits",
                Size = new Size(150, 30),
                Location = new Point(170, 60)
            };
            syncProductsButton.Click += async (s, e) => await SyncProductsAsync();

            controlsPanel.Controls.AddRange(new Control[] { syncAllButton, syncCustomersButton, syncProductsButton });

            // Log de synchronisation
            var logTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                ScrollBars = ScrollBars.Vertical
            };

            // Panel de statut
            var statusPanel = new Panel { Dock = DockStyle.Fill };
            var lastSyncLabel = new Label
            {
                Text = "Dernière synchronisation: Jamais",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            statusPanel.Controls.Add(lastSyncLabel);

            // Boutons d'action
            var actionPanel = new Panel { Dock = DockStyle.Fill };
            var clearLogButton = new Button
            {
                Text = "Effacer le log",
                Width = 100,
                Dock = DockStyle.Right
            };
            clearLogButton.Click += (s, e) => logTextBox.Clear();

            actionPanel.Controls.Add(clearLogButton);

            // Ajouter les contrôles au panel
            panel.Controls.Add(controlsPanel, 0, 0);
            panel.Controls.Add(logTextBox, 0, 1);
            panel.Controls.Add(statusPanel, 0, 2);
            panel.Controls.Add(actionPanel, 0, 3);

            _syncTab.Controls.Add(panel);
        }

        private void CreateSettingsTab()
        {
            _settingsTab = new TabPage("Paramètres");
            
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6
            };

            // Colonnes
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Lignes
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Configuration API
            panel.Controls.Add(new Label { Text = "URL API PPG Live:", Dock = DockStyle.Fill }, 0, 0);
            var apiUrlTextBox = new TextBox { Dock = DockStyle.Fill };
            panel.Controls.Add(apiUrlTextBox, 1, 0);

            panel.Controls.Add(new Label { Text = "Clé API:", Dock = DockStyle.Fill }, 0, 1);
            var apiKeyTextBox = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true };
            panel.Controls.Add(apiKeyTextBox, 1, 1);

            panel.Controls.Add(new Label { Text = "Secret API:", Dock = DockStyle.Fill }, 0, 2);
            var apiSecretTextBox = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true };
            panel.Controls.Add(apiSecretTextBox, 1, 2);

            panel.Controls.Add(new Label { Text = "Timeout (secondes):", Dock = DockStyle.Fill }, 0, 3);
            var timeoutTextBox = new TextBox { Dock = DockStyle.Fill };
            panel.Controls.Add(timeoutTextBox, 1, 3);

            // Boutons d'action
            var buttonPanel = new Panel { Dock = DockStyle.Fill };
            var saveButton = new Button
            {
                Text = "Sauvegarder",
                Width = 100,
                Dock = DockStyle.Left
            };
            var testButton = new Button
            {
                Text = "Tester la connexion",
                Width = 150,
                Dock = DockStyle.Left
            };

            buttonPanel.Controls.Add(testButton);
            buttonPanel.Controls.Add(saveButton);

            panel.Controls.Add(buttonPanel, 0, 4);
            panel.SetColumnSpan(buttonPanel, 2);

            _settingsTab.Controls.Add(panel);
        }

        private void CreateStatusStrip()
        {
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("Prêt");
            _progressBar = new ToolStripProgressBar
            {
                Visible = false
            };

            _statusStrip.Items.Add(_statusLabel);
            _statusStrip.Items.Add(_progressBar);
        }

        private async void LoadInitialData()
        {
            try
            {
                UpdateStatus("Chargement des données...", true);
                
                // Charger les données initiales selon l'onglet actif
                switch (_mainTabControl.SelectedIndex)
                {
                    case 0: // Clients
                        await LoadCustomersAsync();
                        break;
                    case 1: // Produits
                        await LoadProductsAsync();
                        break;
                    case 2: // Commandes
                        await LoadOrdersAsync();
                        break;
                    case 3: // Factures
                        await LoadInvoicesAsync();
                        break;
                    case 4: // Devis
                        await LoadQuotationsAsync();
                        break;
                }
                
                UpdateStatus("Données chargées", false);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Erreur: {ex.Message}", false);
                MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SearchCustomersAsync(string searchText)
        {
            try
            {
                UpdateStatus("Recherche de clients...", true);
                
                var criteria = new CustomerSearchCriteria
                {
                    Name = searchText,
                    PageSize = 100
                };
                
                var customers = await _plugin.SearchCustomersAsync(criteria);
                
                // Mettre à jour la liste des clients
                var listBox = GetControlInTab<ListBox>(_customersTab, 0);
                listBox.Items.Clear();
                foreach (var customer in customers)
                {
                    listBox.Items.Add($"{customer.Code} - {customer.Name}");
                }
                
                UpdateStatus($"{customers.Count} clients trouvés", false);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Erreur: {ex.Message}", false);
                MessageBox.Show($"Erreur lors de la recherche: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SearchProductsAsync(string searchText)
        {
            try
            {
                UpdateStatus("Recherche de produits...", true);
                
                var criteria = new ProductSearchCriteria
                {
                    Name = searchText,
                    PageSize = 100
                };
                
                var response = await _productService.SearchProductsAsync(criteria);
                
                // Mettre à jour la liste des produits
                var listBox = GetControlInTab<ListBox>(_productsTab, 0);
                listBox.Items.Clear();
                if (response.Success)
                {
                    foreach (var product in response.Data.Data)
                    {
                        listBox.Items.Add($"{product.Code} - {product.Name}");
                    }
                }
                
                UpdateStatus("Recherche terminée", false);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Erreur: {ex.Message}", false);
                MessageBox.Show($"Erreur lors de la recherche: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadCustomersAsync()
        {
            // Implémentation du chargement des clients
            UpdateStatus("Chargement des clients...", true);
            await Task.Delay(1000); // Simulation
            UpdateStatus("Clients chargés", false);
        }

        private async Task LoadProductsAsync()
        {
            // Implémentation du chargement des produits
            UpdateStatus("Chargement des produits...", true);
            await Task.Delay(1000); // Simulation
            UpdateStatus("Produits chargés", false);
        }

        private async Task LoadOrdersAsync()
        {
            // Implémentation du chargement des commandes
            UpdateStatus("Chargement des commandes...", true);
            await Task.Delay(1000); // Simulation
            UpdateStatus("Commandes chargées", false);
        }

        private async Task LoadInvoicesAsync()
        {
            // Implémentation du chargement des factures
            UpdateStatus("Chargement des factures...", true);
            await Task.Delay(1000); // Simulation
            UpdateStatus("Factures chargées", false);
        }

        private async Task LoadQuotationsAsync()
        {
            // Implémentation du chargement des devis
            UpdateStatus("Chargement des devis...", true);
            await Task.Delay(1000); // Simulation
            UpdateStatus("Devis chargés", false);
        }

        private async Task SyncCustomersAsync()
        {
            try
            {
                UpdateStatus("Synchronisation des clients...", true);
                
                var result = await _plugin.SyncCustomersAsync();
                
                if (result.Status == "Completed")
                {
                    UpdateStatus($"Synchronisation réussie: {result.SuccessRecords} clients", false);
                    MessageBox.Show($"Synchronisation réussie!\n{result.SuccessRecords} clients synchronisés", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateStatus($"Synchronisation avec erreurs: {result.ErrorRecords} erreurs", false);
                    MessageBox.Show($"Synchronisation terminée avec des erreurs.\n{result.ErrorRecords} erreurs sur {result.TotalRecords} enregistrements", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Erreur: {ex.Message}", false);
                MessageBox.Show($"Erreur lors de la synchronisation: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SyncProductsAsync()
        {
            try
            {
                UpdateStatus("Synchronisation des produits...", true);
                
                var result = await _plugin.SyncProductsAsync();
                
                if (result.Status == "Completed")
                {
                    UpdateStatus($"Synchronisation réussie: {result.SuccessRecords} produits", false);
                    MessageBox.Show($"Synchronisation réussie!\n{result.SuccessRecords} produits synchronisés", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateStatus($"Synchronisation avec erreurs: {result.ErrorRecords} erreurs", false);
                    MessageBox.Show($"Synchronisation terminée avec des erreurs.\n{result.ErrorRecords} erreurs sur {result.TotalRecords} enregistrements", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Erreur: {ex.Message}", false);
                MessageBox.Show($"Erreur lors de la synchronisation: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SyncAllAsync()
        {
            try
            {
                UpdateStatus("Synchronisation complète...", true);
                
                var result = await _plugin.SynchronizeAllAsync();
                
                if (result.Status == "Completed")
                {
                    UpdateStatus($"Synchronisation complète réussie: {result.SuccessRecords} enregistrements", false);
                    MessageBox.Show($"Synchronisation complète réussie!\n{result.SuccessRecords} enregistrements synchronisés", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateStatus($"Synchronisation avec erreurs: {result.ErrorRecords} erreurs", false);
                    MessageBox.Show($"Synchronisation terminée avec des erreurs.\n{result.ErrorRecords} erreurs sur {result.TotalRecords} enregistrements", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Erreur: {ex.Message}", false);
                MessageBox.Show($"Erreur lors de la synchronisation: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string message, bool showProgress)
        {
            _statusLabel.Text = message;
            _progressBar.Visible = showProgress;
            Application.DoEvents();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _plugin?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
