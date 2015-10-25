﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram
{
    public class EditPanel : Panel
    {
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom

        public bool editing = false; // panel je zobrazený
        public TextBox edit = null; // edit pre nove meno nody

        public Node prevSelectedNode = null;
        public Node editedNode = null;

        public EditPanel(DiagramView diagramView)
        {
            this.diagramView = diagramView;

            InitializeComponent();

            this.Hide();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.edit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            //
            // edit
            //
            this.edit.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFB8");
            this.edit.BorderStyle = BorderStyle.None;
            this.edit.Location = new System.Drawing.Point(4, 4);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(100, 13);
            this.edit.TabIndex = 0;
            this.edit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nodeNameEdit_KeyDown);
            this.edit.TextChanged += new EventHandler(nodeNameEdit_TextChanged);
            this.edit.AcceptsReturn = true;
            this.edit.AcceptsTab = true;
            this.edit.Multiline = true;
            this.edit.Left = 12;
            this.edit.Top = 8;

            //
            // EditPanel
            //
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFB8");
            this.Controls.Add(this.edit);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        // EDITPANEL SHOW add new node
        public void showEditPanel(Position currentPosition, char FirstKey = ' ')
        {
            if (!this.Visible)
            {
                this.Left = currentPosition.x;
                this.Top = currentPosition.y;

                
                Font f = this.diagramView.diagram.FontDefault;

                Font defaultFont = this.diagramView.diagram.FontDefault;
                Font font = new Font(defaultFont.FontFamily, defaultFont.Size / this.diagramView.scale, defaultFont.Style);

                this.edit.Font = font;
                
                this.edit.Text = "" + (FirstKey).ToString(); // add first character

                SizeF s = Fonts.MeasureString(edit.Text.ToUpper(), edit.Font);

                this.edit.Height = (int)Math.Round(s.Height, 0) + 20;
                this.Height = this.edit.Height;

                this.edit.Width = (int)Math.Round(s.Width, 0) + 20;
                this.Width = this.edit.Width;

                this.editing = true;
                this.Show();
                this.edit.Show();
                this.edit.Focus();
                this.edit.SelectionStart = edit.Text.Length;
            }
        }

        // EDITPANEL SHOW edit existing node
        public void editNode(Position currentPosition, Node editedNode)
        {
            if (!this.Visible)
            {
                this.editedNode = editedNode;
                this.editedNode.visible = false;
                this.diagramView.diagram.InvalidateDiagram();

                this.Left = currentPosition.x;
                this.Top = currentPosition.y;

                Font nodeFont = this.editedNode.font;
                Font font = new Font(nodeFont.FontFamily, nodeFont.Size / this.diagramView.scale, nodeFont.Style);

                this.edit.Font = font;
                this.edit.Text = this.editedNode.text; // add first character

                SizeF s = Fonts.MeasureString(edit.Text.ToUpper(), edit.Font);

                this.edit.Height = (int)Math.Round(s.Height, 0) + 20;
                this.Height = this.edit.Height;

                this.edit.Width = (int)Math.Round(s.Width, 0) + 20;
                this.Width = this.edit.Width;

                this.editing = true;
                this.Show();
                this.edit.Show();
                this.edit.Focus();
                this.edit.SelectionStart = edit.Text.Length;
            }
        }

        // EDITPANEL SAVE
        public void saveNodeNamePanel()
        {
            if (edit.Text!="")
            {
                if (this.editedNode == null) {
                    this.editedNode = this.diagramView.CreateNode(this.Left, this.Top);
                }

                this.editedNode.text = edit.Text;
                this.editedNode.font = this.diagramView.diagram.FontDefault;
                SizeF s = this.diagramView.diagram.MeasureStringWithMargin(this.editedNode.text, this.editedNode.font);
                this.editedNode.width = (int)s.Width;
                this.editedNode.height = (int)s.Height;
                this.editedNode.visible = true;

                if (this.prevSelectedNode != null)
                {
                    this.diagramView.diagram.Connect(this.prevSelectedNode, this.editedNode);
                    this.prevSelectedNode = null;
                }

                this.diagramView.diagram.unsave();
            }

            this.Hide();
            this.editedNode = null;
            editing = false;
            this.diagramView.diagram.InvalidateDiagram();
            this.diagramView.Focus();
        }

        // EDITPANEL RESIZE change panel with after text change
        private void nodeNameEdit_TextChanged(object sender, EventArgs e)
        {
            SizeF size = Fonts.MeasureString(edit.Text, edit.Font);

            // Resize the textbox
            this.edit.Width = (int)Math.Round(size.Width, 0) + 20;
            this.Width = this.edit.Width;
        }

        // EDITPANEL EDIT catch keys in edit
        private void nodeNameEdit_KeyDown(object sender, KeyEventArgs e)
        {

            Keys keyData = e.KeyCode;

            if (KeyMap.parseKey("ESCAPE", keyData)) // zrusenie editácie v panely
            {
                this.saveNodeNamePanel();
                this.Focus();
            }

            if (KeyMap.parseKey("ENTER", keyData)) // zvretie panelu a vytvorenie novej editacie
            {
                this.saveNodeNamePanel();
                this.Focus();
            }

            if (KeyMap.parseKey("TAB", keyData) && !e.Shift) // zvretie panelu a vytvorenie novej editacie
            {
                this.saveNodeNamePanel();
                this.Focus();
                this.diagramView.addNodeAfterNode();
            }

            if (KeyMap.parseKey("TAB", keyData) && e.Shift) // zvretie panelu a vytvorenie novej editacie
            {
                this.saveNodeNamePanel();
                this.Focus();
                this.diagramView.addNodeBelowNode();
            }

            if (KeyMap.parseKey("ENTER", keyData) || KeyMap.parseKey("TAB", keyData))
            {
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        public bool isEditing()
        {
            return this.editing;
        }
    }
}