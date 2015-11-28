﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public class Popup : ContextMenuStrip
    {
        public DiagramView diagramView = null;       // diagram ktory je previazany z pohladom

        private System.Windows.Forms.ToolStripMenuItem editItem; //EDIT
        private System.Windows.Forms.ToolStripMenuItem colorItem;
        private System.Windows.Forms.ToolStripMenuItem removeShortcutItem;
        private System.Windows.Forms.ToolStripMenuItem linkItem; // LINK
        private System.Windows.Forms.ToolStripMenuItem openlinkItem;
        private System.Windows.Forms.ToolStripMenuItem openLinkDirectoryItem;

        private System.Windows.Forms.ToolStripMenuItem alignItem; // ALIGN
        private System.Windows.Forms.ToolStripMenuItem leftItem;
        private System.Windows.Forms.ToolStripMenuItem rightItem;
        private System.Windows.Forms.ToolStripMenuItem toLineItem;
        private System.Windows.Forms.ToolStripMenuItem inColumnItem;
        private System.Windows.Forms.ToolStripMenuItem groupItem;

        private System.Windows.Forms.ToolStripSeparator quickActionSeparator; //SEPARATOR

        private System.Windows.Forms.ToolStripMenuItem fileItem; // FILE
        private System.Windows.Forms.ToolStripMenuItem newItem;
        private System.Windows.Forms.ToolStripMenuItem saveItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsItem;
        private System.Windows.Forms.ToolStripMenuItem exportItem; //EXPORT
        private System.Windows.Forms.ToolStripMenuItem textItem;
        private System.Windows.Forms.ToolStripMenuItem exportToPngItem;
        private System.Windows.Forms.ToolStripMenuItem openItem;
        private System.Windows.Forms.ToolStripMenuItem openDiagramDirectoryItem;
        private System.Windows.Forms.ToolStripMenuItem exitItem;

        private System.Windows.Forms.ToolStripMenuItem editMenuItem; // EDIT
        private System.Windows.Forms.ToolStripMenuItem copyItem;
        private System.Windows.Forms.ToolStripMenuItem cutItem;
        private System.Windows.Forms.ToolStripMenuItem pasteItem;
        private System.Windows.Forms.ToolStripSeparator editSeparator; // SEPARATOR
        private System.Windows.Forms.ToolStripMenuItem copyLinkItem;
        private System.Windows.Forms.ToolStripMenuItem copyNoteItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToLinkItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToNoteItem;

        private System.Windows.Forms.ToolStripMenuItem viewItem; // VIEW
        private System.Windows.Forms.ToolStripMenuItem newViewItem;
        private System.Windows.Forms.ToolStripMenuItem centerItem;
        private System.Windows.Forms.ToolStripMenuItem setStartPositionItem;

        private System.Windows.Forms.ToolStripMenuItem layerItem;  // LAYER
        private System.Windows.Forms.ToolStripMenuItem inItem;
        private System.Windows.Forms.ToolStripMenuItem outItem;

        private System.Windows.Forms.ToolStripSeparator helpSeparator;

        private System.Windows.Forms.ToolStripMenuItem optionItem; // OPTION
        private System.Windows.Forms.ToolStripMenuItem encryptItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordItem;
        private System.Windows.Forms.ToolStripMenuItem readonlyItem;
        private System.Windows.Forms.ToolStripMenuItem gridItem;
        private System.Windows.Forms.ToolStripMenuItem coordinatesItem;
        private System.Windows.Forms.ToolStripMenuItem bordersItem;
        private System.Windows.Forms.ToolStripMenuItem defaultFontItem;
        private System.Windows.Forms.ToolStripMenuItem resetFontItem;

        private System.Windows.Forms.ToolStripMenuItem helpItem; // HELP
        private System.Windows.Forms.ToolStripMenuItem consoleItem;
        private System.Windows.Forms.ToolStripMenuItem visitWebsiteItem;
        private System.Windows.Forms.ToolStripMenuItem releaseNoteItem;
        private System.Windows.Forms.ToolStripMenuItem aboutItem;

        public Popup(System.ComponentModel.IContainer container, DiagramView diagramView) : base(container)
        {
            this.diagramView = diagramView;

            InitializeComponent();

#if DEBUG
            this.consoleItem.Visible = true;
            this.coordinatesItem.Visible = true;
#endif

            this.gridItem.Checked = this.diagramView.diagram.options.grid;
            this.bordersItem.Checked = this.diagramView.diagram.options.borders;
            this.coordinatesItem.Checked = this.diagramView.diagram.options.coordinates;
            this.readonlyItem.Checked = this.diagramView.diagram.options.readOnly;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.editItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeShortcutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openlinkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLinkDirectoryItem = new System.Windows.Forms.ToolStripMenuItem();

            this.alignItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toLineItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inColumnItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupItem = new System.Windows.Forms.ToolStripMenuItem();

            this.quickActionSeparator = new System.Windows.Forms.ToolStripSeparator();

            this.fileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToPngItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDiagramDirectoryItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();

            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.copyLinkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyNoteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToLinkItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToNoteItem = new System.Windows.Forms.ToolStripMenuItem();

            this.viewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newViewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setStartPositionItem = new System.Windows.Forms.ToolStripMenuItem();
            
            this.layerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outItem = new System.Windows.Forms.ToolStripMenuItem();

            this.helpSeparator = new System.Windows.Forms.ToolStripSeparator();

            this.optionItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readonlyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinatesItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bordersItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultFontItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetFontItem = new System.Windows.Forms.ToolStripMenuItem();

            this.helpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitWebsiteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseNoteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutItem = new System.Windows.Forms.ToolStripMenuItem();

            // 
            // PopupMenu
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editItem,
            this.colorItem,
            this.removeShortcutItem,
            this.linkItem,
            this.alignItem,
            this.quickActionSeparator,
            this.fileItem,
            this.editMenuItem,
            this.viewItem,
            this.layerItem,
            this.helpSeparator,
            this.optionItem,
            this.helpItem});
            this.Name = "popupMenu";
            this.Size = new System.Drawing.Size(165, 280);
            this.Opening += new System.ComponentModel.CancelEventHandler(this.PopupMenu_Opening);
            // 
            // editItem
            // 
            this.editItem.Name = "editItem";
            this.editItem.Size = new System.Drawing.Size(164, 22);
            this.editItem.Text = "Edit";
            this.editItem.Click += new System.EventHandler(this.editItem_Click);
            // 
            // colorItem
            // 
            this.colorItem.Name = "colorItem";
            this.colorItem.Size = new System.Drawing.Size(164, 22);
            this.colorItem.Text = "Color";
            this.colorItem.Click += new System.EventHandler(this.colorItem_Click);
            // 
            // removeShortcutItem
            // 
            this.removeShortcutItem.Name = "removeShortcutItem";
            this.removeShortcutItem.Size = new System.Drawing.Size(164, 22);
            this.removeShortcutItem.Text = "Remove shortcut";
            this.removeShortcutItem.Click += new System.EventHandler(this.removeShortcutItem_Click);
            // 
            // linkItem
            // 
            this.linkItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openlinkItem,
            this.copyLinkItem,
            this.openLinkDirectoryItem});
            this.linkItem.Name = "linkItem";
            this.linkItem.Size = new System.Drawing.Size(164, 22);
            this.linkItem.Text = "Link";
            // 
            // openlinkItem
            // 
            this.openlinkItem.Name = "openlinkItem";
            this.openlinkItem.Size = new System.Drawing.Size(153, 22);
            this.openlinkItem.Text = "Open";
            this.openlinkItem.Click += new System.EventHandler(this.openlinkItem_Click);
            // 
            // openLinkDirectoryItem
            // 
            this.openLinkDirectoryItem.Name = "openLinkDirectoryItem";
            this.openLinkDirectoryItem.Size = new System.Drawing.Size(153, 22);
            this.openLinkDirectoryItem.Text = "Open directory";
            this.openLinkDirectoryItem.Click += new System.EventHandler(this.openLinkDirectoryItem_Click);
            // 
            // alignItem
            // 
            this.alignItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftItem,
            this.rightItem,
            this.toLineItem,
            this.inColumnItem,
            this.groupItem});
            this.alignItem.Name = "alignItem";
            this.alignItem.Size = new System.Drawing.Size(164, 22);
            this.alignItem.Text = "Align";
            // 
            // leftItem
            // 
            this.leftItem.Name = "leftItem";
            this.leftItem.Size = new System.Drawing.Size(128, 22);
            this.leftItem.Text = "Left";
            this.leftItem.Click += new System.EventHandler(this.leftItem_Click);
            // 
            // rightItem
            // 
            this.rightItem.Name = "rightItem";
            this.rightItem.Size = new System.Drawing.Size(128, 22);
            this.rightItem.Text = "Right";
            this.rightItem.Click += new System.EventHandler(this.rightItem_Click);
            // 
            // toLineItem
            // 
            this.toLineItem.Name = "toLineItem";
            this.toLineItem.Size = new System.Drawing.Size(128, 22);
            this.toLineItem.Text = "To line";
            this.toLineItem.Click += new System.EventHandler(this.toLineItem_Click);
            // 
            // inColumnItem
            // 
            this.inColumnItem.Name = "inColumnItem";
            this.inColumnItem.Size = new System.Drawing.Size(128, 22);
            this.inColumnItem.Text = "In column";
            this.inColumnItem.Click += new System.EventHandler(this.inColumnItem_Click);
            // 
            // groupItem
            // 
            this.groupItem.Name = "groupItem";
            this.groupItem.Size = new System.Drawing.Size(128, 22);
            this.groupItem.Text = "Group";
            this.groupItem.Click += new System.EventHandler(this.groupItem_Click);
            // 
            // quickActionSeparator
            // 
            this.quickActionSeparator.Name = "quickActionSeparator";
            this.quickActionSeparator.Size = new System.Drawing.Size(161, 6);
            // 
            // fileItem
            // 
            this.fileItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newItem,
            this.saveItem,
            this.saveAsItem,
            this.exportItem,
            this.openItem,
            this.openDiagramDirectoryItem,
            this.exitItem});
            this.fileItem.Name = "fileItem";
            this.fileItem.Size = new System.Drawing.Size(164, 22);
            this.fileItem.Text = "File";
            // 
            // newItem
            // 
            this.newItem.Name = "newItem";
            this.newItem.Size = new System.Drawing.Size(154, 22);
            this.newItem.Text = "New";
            this.newItem.Click += new System.EventHandler(this.newItem_Click);
            // 
            // saveItem
            // 
            this.saveItem.Name = "saveItem";
            this.saveItem.Size = new System.Drawing.Size(154, 22);
            this.saveItem.Text = "Save";
            this.saveItem.Click += new System.EventHandler(this.saveItem_Click);
            // 
            // saveAsItem
            // 
            this.saveAsItem.Name = "saveAsItem";
            this.saveAsItem.Size = new System.Drawing.Size(154, 22);
            this.saveAsItem.Text = "Save As";
            this.saveAsItem.Click += new System.EventHandler(this.saveAsItem_Click);
            // 
            // exportItem
            // 
            this.exportItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textItem,
            this.exportToPngItem});
            this.exportItem.Name = "exportItem";
            this.exportItem.Size = new System.Drawing.Size(154, 22);
            this.exportItem.Text = "Export";
            // 
            // textItem
            // 
            this.textItem.Name = "textItem";
            this.textItem.Size = new System.Drawing.Size(145, 22);
            this.textItem.Text = "Text";
            this.textItem.Click += new System.EventHandler(this.textItem_Click);
            // 
            // exportToPngItem
            // 
            this.exportToPngItem.Name = "exportToPngItem";
            this.exportToPngItem.Size = new System.Drawing.Size(145, 22);
            this.exportToPngItem.Text = "Export to png";
            this.exportToPngItem.Click += new System.EventHandler(this.exportToPngItem_Click);
            // 
            // openItem
            // 
            this.openItem.Name = "openItem";
            this.openItem.Size = new System.Drawing.Size(154, 22);
            this.openItem.Text = "Open";
            this.openItem.Click += new System.EventHandler(this.openItem_Click);
            // 
            // openDiagramDirectoryItem
            // 
            this.openDiagramDirectoryItem.Name = "openDiagramDirectoryItem";
            this.openDiagramDirectoryItem.Size = new System.Drawing.Size(154, 22);
            this.openDiagramDirectoryItem.Text = "Open Directory";
            this.openDiagramDirectoryItem.Click += new System.EventHandler(this.openDiagramDirectoryItem_Click);
            // 
            // exitItem
            // 
            this.exitItem.Name = "exitItem";
            this.exitItem.Size = new System.Drawing.Size(154, 22);
            this.exitItem.Text = "Exit";
            this.exitItem.Click += new System.EventHandler(this.exitItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyItem,
            this.cutItem,
            this.pasteItem,
            this.editSeparator,
            this.copyLinkItem,
            this.copyNoteItem,
            this.pasteToLinkItem,
            this.pasteToNoteItem
            });
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(164, 22);
            this.editMenuItem.Text = "Edit";
            // 
            // copyItem
            // 
            this.copyItem.Name = "editItem";
            this.copyItem.Size = new System.Drawing.Size(164, 22);
            this.copyItem.Text = "Copy";
            this.copyItem.Click += new System.EventHandler(this.copyItem_Click);
            // 
            // cutItem
            // 
            this.cutItem.Name = "editItem";
            this.cutItem.Size = new System.Drawing.Size(164, 22);
            this.cutItem.Text = "Cut";
            this.cutItem.Click += new System.EventHandler(this.cutItem_Click);
            // 
            // pasteItem
            // 
            this.pasteItem.Name = "editItem";
            this.pasteItem.Size = new System.Drawing.Size(164, 22);
            this.pasteItem.Text = "Paste";
            this.pasteItem.Click += new System.EventHandler(this.pasteItem_Click);
            // 
            // editSeparator
            // 
            this.editSeparator.Name = "editSeparator";
            this.editSeparator.Size = new System.Drawing.Size(161, 6);
            // 
            // copyLinkItem
            // 
            this.copyLinkItem.Name = "copyLinkItem";
            this.copyLinkItem.Size = new System.Drawing.Size(164, 22);
            this.copyLinkItem.Text = "Copy link";
            this.copyLinkItem.Click += new System.EventHandler(this.copyLinkItem_Click);
            // 
            // copyNoteItem
            // 
            this.copyNoteItem.Name = "copyNoteItem";
            this.copyNoteItem.Size = new System.Drawing.Size(164, 22);
            this.copyNoteItem.Text = "Copy note";
            this.copyNoteItem.Click += new System.EventHandler(this.copyNoteItem_Click);
            // 
            // pasteToLinkItem
            // 
            this.pasteToLinkItem.Name = "pasteToLinkItem";
            this.pasteToLinkItem.Size = new System.Drawing.Size(164, 22);
            this.pasteToLinkItem.Text = "Paste to link";
            this.pasteToLinkItem.Click += new System.EventHandler(this.pasteToLinkItem_Click);
            // 
            // pasteToNoteItem
            // 
            this.pasteToNoteItem.Name = "pasteToNoteItem";
            this.pasteToNoteItem.Size = new System.Drawing.Size(164, 22);
            this.pasteToNoteItem.Text = "Paste to note";
            this.pasteToNoteItem.Click += new System.EventHandler(this.pasteToNoteItem_Click);
            // 
            // viewItem
            // 
            this.viewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newViewItem,
            this.centerItem,
            this.setStartPositionItem});
            this.viewItem.Name = "viewItem";
            this.viewItem.Size = new System.Drawing.Size(164, 22);
            this.viewItem.Text = "View";
            // 
            // newViewItem
            // 
            this.newViewItem.Name = "newViewItem";
            this.newViewItem.Size = new System.Drawing.Size(126, 22);
            this.newViewItem.Text = "New View";
            this.newViewItem.Click += new System.EventHandler(this.newViewItem_Click);
            // 
            // centerItem
            // 
            this.centerItem.Name = "centerItem";
            this.centerItem.Size = new System.Drawing.Size(162, 22);
            this.centerItem.Text = "Center";
            this.centerItem.Click += new System.EventHandler(this.centerItem_Click);
            // 
            // setStartPositionItem
            // 
            this.setStartPositionItem.Name = "setStartPositionItem";
            this.setStartPositionItem.Size = new System.Drawing.Size(162, 22);
            this.setStartPositionItem.Text = "Set start position";
            this.setStartPositionItem.Click += new System.EventHandler(this.setStartPositionItem_Click);
            // 
            // layerItem
            // 
            this.layerItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inItem,
            this.outItem});
            this.layerItem.Name = "layerItem";
            this.layerItem.Size = new System.Drawing.Size(164, 22);
            this.layerItem.Text = "Layer";
            // 
            // inItem
            // 
            this.inItem.Name = "inItem";
            this.inItem.Size = new System.Drawing.Size(94, 22);
            this.inItem.Text = "In";
            this.inItem.Click += new System.EventHandler(this.inItem_Click);
            // 
            // outItem
            // 
            this.outItem.Name = "outItem";
            this.outItem.Size = new System.Drawing.Size(94, 22);
            this.outItem.Text = "Out";
            this.outItem.Click += new System.EventHandler(this.outItem_Click);
            // 
            // helpSeparator
            // 
            this.helpSeparator.Name = "helpSeparator";
            this.helpSeparator.Size = new System.Drawing.Size(161, 6);
            // 
            // optionItem
            // 
            this.optionItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encryptItem,
            this.changePasswordItem,
            this.readonlyItem,
            this.gridItem,
            this.coordinatesItem,
            this.bordersItem,
            this.defaultFontItem,
            this.resetFontItem});
            this.optionItem.Name = "optionItem";
            this.optionItem.Size = new System.Drawing.Size(164, 22);
            this.optionItem.Text = "Option";
            // 
            // encryptItem
            // 
            this.encryptItem.Name = "encryptItem";
            this.encryptItem.Size = new System.Drawing.Size(168, 22);
            this.encryptItem.Text = "Encrypt";
            this.encryptItem.Click += new System.EventHandler(this.encryptItem_Click);
            // 
            // changePasswordItem
            // 
            this.changePasswordItem.Name = "changePasswordItem";
            this.changePasswordItem.Size = new System.Drawing.Size(168, 22);
            this.changePasswordItem.Text = "Change password";
            this.changePasswordItem.Click += new System.EventHandler(this.changePasswordItem_Click);
            // 
            // readonlyItem
            // 
            this.readonlyItem.CheckOnClick = true;
            this.readonlyItem.Name = "readonlyItem";
            this.readonlyItem.Size = new System.Drawing.Size(168, 22);
            this.readonlyItem.Text = "Read only";
            this.readonlyItem.Click += new System.EventHandler(this.readonlyItem_Click);
            // 
            // gridItem
            // 
            this.gridItem.Checked = true;
            this.gridItem.CheckOnClick = true;
            this.gridItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridItem.Name = "gridItem";
            this.gridItem.Size = new System.Drawing.Size(168, 22);
            this.gridItem.Text = "Grid";
            this.gridItem.Click += new System.EventHandler(this.gridItem_Click);
            // 
            // coordinatesItem
            // 
            this.coordinatesItem.CheckOnClick = true;
            this.coordinatesItem.Name = "coordinatesItem";
            this.coordinatesItem.Size = new System.Drawing.Size(168, 22);
            this.coordinatesItem.Text = "Coordinates";
            this.coordinatesItem.Visible = false;
            this.coordinatesItem.Click += new System.EventHandler(this.coordinatesItem_Click);
            // 
            // bordersItem
            // 
            this.bordersItem.CheckOnClick = true;
            this.bordersItem.Name = "bordersItem";
            this.bordersItem.Size = new System.Drawing.Size(168, 22);
            this.bordersItem.Text = "Borders";
            this.bordersItem.Click += new System.EventHandler(this.bordersItem_Click);
            // 
            // defaultFontItem
            // 
            this.defaultFontItem.Name = "defaultFontItem";
            this.defaultFontItem.Size = new System.Drawing.Size(168, 22);
            this.defaultFontItem.Text = "Default font";
            this.defaultFontItem.Click += new System.EventHandler(this.defaultFontItem_Click);
            // 
            // resetFontItem
            // 
            this.resetFontItem.Name = "resetFontItem";
            this.resetFontItem.Size = new System.Drawing.Size(168, 22);
            this.resetFontItem.Text = "Reset font";
            this.resetFontItem.Click += new System.EventHandler(this.resetFontItem_Click);
            // 
            // helpItem
            // 
            this.helpItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleItem,
            this.visitWebsiteItem,
            this.releaseNoteItem,
            this.aboutItem});
            this.helpItem.Name = "helpItem";
            this.helpItem.Size = new System.Drawing.Size(164, 22);
            this.helpItem.Text = "Help";
            // 
            // consoleItem
            // 
            this.consoleItem.Name = "consoleItem";
            this.consoleItem.Size = new System.Drawing.Size(155, 22);
            this.consoleItem.Text = "Debug Console";
            this.consoleItem.Visible = false;
            this.consoleItem.Click += new System.EventHandler(this.consoleItem_Click);
            // 
            // visitWebsiteItem
            // 
            this.visitWebsiteItem.Name = "visitWebsiteItem";
            this.visitWebsiteItem.Size = new System.Drawing.Size(155, 22);
            this.visitWebsiteItem.Text = "Visit homesite";
            this.visitWebsiteItem.Click += new System.EventHandler(this.visitWebsiteItem_Click);
            // 
            // releaseNoteItem
            // 
            this.releaseNoteItem.Name = "releaseNoteItem";
            this.releaseNoteItem.Size = new System.Drawing.Size(155, 22);
            this.releaseNoteItem.Text = "Release Note";
            this.releaseNoteItem.Click += new System.EventHandler(this.releaseNoteItem_Click);
            // 
            // aboutItem
            // 
            this.aboutItem.Name = "aboutItem";
            this.aboutItem.Size = new System.Drawing.Size(155, 22);
            this.aboutItem.Text = "About";
            this.aboutItem.Click += new System.EventHandler(this.aboutItem_Click);
            // 
            // Popup
            // 
            this.ResumeLayout(false);
        }
        #endregion

        /*************************************************************************************************************************/

        // MENU Manage                                                                                // POPUP MENU
        public void PopupMenu_Opening(object sender, CancelEventArgs e)
        {
            // NEW FILE
            if (this.diagramView.diagram.isNew())
            {
                this.openDiagramDirectoryItem.Enabled = false;
            }
            else
            {
                this.openDiagramDirectoryItem.Enabled = true;
            }

            // SELECTION EMPTY
            if (this.diagramView.SelectedNodes.Count() == 0)
            {
                editItem.Visible = false;
                colorItem.Visible = false;
                linkItem.Visible = false;
                openlinkItem.Enabled = false;
                quickActionSeparator.Visible = false;//separator
                alignItem.Visible = false;
                removeShortcutItem.Visible = false;
                openLinkDirectoryItem.Visible = false;
                copyItem.Enabled = false;
                cutItem.Enabled = false;
                copyLinkItem.Enabled = false;
                copyNoteItem.Enabled = false;
            }

            // SELECTION NOT EMPTY
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                editItem.Visible = true;
                colorItem.Visible = true;
                quickActionSeparator.Visible = true;//separator
                copyItem.Enabled = true;
                cutItem.Enabled = true;
                copyLinkItem.Enabled = true;
                copyNoteItem.Enabled = true;
            }

            // SELECTION ONE
            if (this.diagramView.SelectedNodes.Count() == 1)
            {
                linkItem.Visible = this.diagramView.SelectedNodes[0].link.Trim() != "";
                copyLinkItem.Enabled = this.diagramView.SelectedNodes[0].link.Trim() != "";
                openlinkItem.Enabled = this.diagramView.SelectedNodes[0].link.Trim() != "";
                alignItem.Visible = false;
                openLinkDirectoryItem.Visible = false;
                if (this.diagramView.SelectedNodes[0].link.Trim().Length > 0 && File.Exists(this.diagramView.SelectedNodes[0].link))
                    openLinkDirectoryItem.Visible = true;
            }

            // SELECTION MORE THEN ONE
            if (this.diagramView.SelectedNodes.Count() > 1)
            {
                linkItem.Visible = false;
                openlinkItem.Enabled = false;
                alignItem.Visible = true;
                removeShortcutItem.Visible = false;
                openLinkDirectoryItem.Visible = false;
            }

            // REMOVE SHORTCUT
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                bool hasShortcut = false;
                foreach (Node node in this.diagramView.SelectedNodes)
                {
                    if (node.shortcut > 0)
                    {
                        hasShortcut = true;
                        break;
                    }
                }

                if (hasShortcut)
                {
                    removeShortcutItem.Visible = true;
                }
            }

            // PASSWORD IS SET 
            if (this.diagramView.diagram.password == "")
            {
                changePasswordItem.Visible = false;
                encryptItem.Visible = true;
            }
            else
            {
                changePasswordItem.Visible = true;
                encryptItem.Visible = false;
            }

            // CLIPBOARD
            DataObject retrievedData = (DataObject)Clipboard.GetDataObject();
            if (retrievedData.GetDataPresent("DiagramXml")
            || retrievedData.GetDataPresent(DataFormats.Text)
            || Clipboard.ContainsFileDropList()
            || Clipboard.GetDataObject() != null)
            {
                pasteItem.Text = "Paste";
                pasteItem.Enabled = true;

                if (retrievedData.GetDataPresent("DiagramXml")) {
                    pasteItem.Text += " diagram";
                }
                else
                if (retrievedData.GetDataPresent(DataFormats.Text))
                {
                    pasteItem.Text += " text";
                }
                else
                if (Clipboard.ContainsFileDropList())
                {
                    pasteItem.Text += " files";
                }
                else
                if (Clipboard.GetDataObject() != null)
                {
                    IDataObject data = Clipboard.GetDataObject();
                    if (data.GetDataPresent(DataFormats.Bitmap)) {
                        pasteItem.Text += " image";
                    }
                }

            }
            else
            {
                pasteItem.Enabled = false;
            }
        }

        // MENU Edit
        public void editItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() == 1)
            {
                this.diagramView.diagram.EditNode(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU Link Open
        public void openlinkItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.OpenLinkAsync(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU New
        public void newItem_Click(object sender, EventArgs e)
        {
            this.diagramView.main.OpenDiagram();
        }

        // MENU Save
        public void saveItem_Click(object sender, EventArgs e)
        {
            this.diagramView.save();
        }

        // MENU Save As
        public void saveAsItem_Click(object sender, EventArgs e)
        {
            this.diagramView.saveas();
        }

        // MENU export to png
        private void exportToPngItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.exportFile.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.exportDiagramToPng();
            }
        }

        // MENU export to txt
        private void textItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.saveTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.diagramView.exportDiagramToTxt(this.diagramView.saveTextFileDialog.FileName);
            }
        }

        // MENU Open
        public void openItem_Click(object sender, EventArgs e)
        {
            this.diagramView.open();
        }

        // MENU Open Directory  - otvory adresar v ktorom sa nachadza prave otvreny subor
        public void openDiagramDirectoryItem_Click(object sender, EventArgs e)
        {
            this.diagramView.openDiagramDirectory();
        }

        // MENU Encription
        private void encryptItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.newPasswordForm == null)
            {
                this.diagramView.main.newPasswordForm = new NewPasswordForm(this.diagramView.main);
            }

            this.diagramView.main.newPasswordForm.Clear();
            this.diagramView.main.newPasswordForm.ShowDialog();
            if (!this.diagramView.main.newPasswordForm.cancled)
            {
                this.diagramView.diagram.password = this.diagramView.main.newPasswordForm.GetPassword();
                this.diagramView.diagram.unsave();
            }
        }

        // MENU Change password
        private void changePasswordItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.changePasswordForm == null)
            {
                this.diagramView.main.changePasswordForm = new ChangePasswordForm(this.diagramView.main);
            }

            this.diagramView.main.changePasswordForm.Clear();
            this.diagramView.main.changePasswordForm.oldpassword = this.diagramView.diagram.password;
            this.diagramView.main.changePasswordForm.ShowDialog();
            if (!this.diagramView.main.changePasswordForm.cancled)
            {
                this.diagramView.diagram.password = this.diagramView.main.changePasswordForm.GetPassword();
                this.diagramView.diagram.unsave();
            }
        }

        // MENU Console
        public void consoleItem_Click(object sender, EventArgs e)
        {
            this.diagramView.showConsole();
        }

        // MENU Exit
        public void exitItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // MENU Copy
        public void copyItem_Click(object sender, EventArgs e)
        {
            this.diagramView.copy();
        }

        // MENU cut
        public void cutItem_Click(object sender, EventArgs e)
        {
            this.diagramView.cut();
        }

        // MENU paste
        public void pasteItem_Click(object sender, EventArgs e)
        {
            this.diagramView.paste();
        }

        // MENU Copy link
        public void copyLinkItem_Click(object sender, EventArgs e)
        {
            this.diagramView.copyLink();
        }

        // MENU Copy note
        public void copyNoteItem_Click(object sender, EventArgs e)
        {
            this.diagramView.copyNote();
        }

        // MENU Copy link
        public void pasteToLinkItem_Click(object sender, EventArgs e)
        {
            this.diagramView.pasteToLink();
        }

        // MENU Copy note
        public void pasteToNoteItem_Click(object sender, EventArgs e)
        {
            this.diagramView.pasteToNote();
        }

        // MENU Center
        public void centerItem_Click(object sender, EventArgs e)
        {
            this.diagramView.GoToHome();
        }

        // MENU set home position
        private void setStartPositionItem_Click(object sender, EventArgs e)
        {
            this.diagramView.setCurentPositionAsHomePosition();
        }

        // MENU Read only
        public void readonlyItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.readOnly = this.readonlyItem.Checked;
        }

        // MENU Grid check
        public void gridItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.grid = this.gridItem.Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Borders
        public void bordersItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.borders = this.bordersItem.Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Option Default font
        public void defaultFontItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectDefaultFont();
        }

        // MENU coordinates
        public void coordinatesItem_Click(object sender, EventArgs e)
        {
            this.diagramView.diagram.options.coordinates = this.coordinatesItem.Checked;
            this.diagramView.diagram.InvalidateDiagram();
        }

        // MENU Change color
        public void colorItem_Click(object sender, EventArgs e)
        {
            this.diagramView.selectColor();
        }

        // MENU VIEW NEW VIEW
        private void newViewItem_Click(object sender, EventArgs e)
        {
            // otvorenie novej insancie DiagramView
            this.diagramView.diagram.openDiagramView();
        }

        // MENU Layer In
        public void inItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() == 1)
            {
                this.diagramView.LayerIn(this.diagramView.SelectedNodes[0]);
            }
        }

        // MENU Layer Out
        public void outItem_Click(object sender, EventArgs e)
        {
            this.diagramView.LayerOut();
        }

        // MENU align left
        private void leftItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignLeft(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align right
        private void rightItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignRight(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to line
        private void toLineItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToLine(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to column
        private void inColumnItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignToColumn(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU align to group to column
        private void groupItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.AlignCompact(this.diagramView.SelectedNodes);
                this.diagramView.diagram.unsave();
                this.diagramView.diagram.InvalidateDiagram();
            }
        }

        // MENU remove shortcut
        private void removeShortcutItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.removeShortcuts(this.diagramView.SelectedNodes);
            }
        }

        // MENU open directory for file in link
        private void openLinkDirectoryItem_Click(object sender, EventArgs e)
        {
            this.diagramView.openLinkDirectory();
        }

        // MENU reset font
        private void resetFontItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.SelectedNodes.Count() > 0)
            {
                this.diagramView.diagram.ResetFont(this.diagramView.SelectedNodes);
            }
            else
            {
                this.diagramView.diagram.ResetFont();
            }
        }

        // MENU visit homepage
        private void visitWebsiteItem_Click(object sender, EventArgs e)
        {
            Network.openUrl(this.diagramView.main.options.home_page);
        }

        // MENU Show release note
        private void releaseNoteItem_Click(object sender, EventArgs e)
        {
            string releasNotePath = Os.getCurrentApplicationDirectory() + Os.getSeparator() + this.diagramView.main.options.release_note;
            if (Os.FileExists(releasNotePath))
            {
                string releaseNoteUrl = "file:///" + Os.toBackslash(Os.getCurrentApplicationDirectory()) + "/" + this.diagramView.main.options.release_note;
                Network.openUrl(releaseNoteUrl);
                Program.log.write("open release note: " + releasNotePath);
            }
            else
            {
                Program.log.write("open release note: error: file not exist" + releasNotePath);
            }
        }

        // MENU show About form
        private void aboutItem_Click(object sender, EventArgs e)
        {
            if (this.diagramView.main.aboutForm == null)
            {
                this.diagramView.main.aboutForm = new AboutForm(this.diagramView.main);
            }

            this.diagramView.main.aboutForm.Show();
        }

    }
}