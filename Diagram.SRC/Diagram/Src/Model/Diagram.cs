﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq; // using
using System.Xml.Linq; // using
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Text;

namespace Diagram
{
    public class Diagram
    {
        public Main main = null;                 // reference to main form

        public Layers layers = new Layers();

        public List<DiagramView> DiagramViews = new List<DiagramView>(); // all views forms to diagram

        // RESOURCES
        public Font FontDefault = null;          // default font

        // ATTRIBUTES File
        public bool NewFile = true;              // flag for new unsaved file without name
        public bool SavedFile = true;            // flag for saved diagram with name
        public string FileName = "";             // path to diagram file

        // ATRIBUTES OBJECTS
        public int maxid = 0;                    // last used node id

        // ATTRIBUTES ENCRYPTION
        public bool encrypted = false;           // flag for encrypted file
        public string password = "";             // password for encrypted file
		private byte[] salt = null;              // salt


        // ATTRIBUTES TextForm
        public List<TextForm> TextWindows = new List<TextForm>();   // opened text textforms for this diagram

        // ATTRIBUTES OPTIONS
        public Options options = new Options();  // diagram options saved to xml file

        public Diagram(Main main)
        {
            this.main = main;
            this.FontDefault = new Font("Open Sans", 10);
        }

        /*************************************************************************************************************************/

        // FILE IS NEW - check if file is empty
        public bool isNew()
        {
            return (this.FileName == "" && this.NewFile && this.SavedFile);
        }

        // FILE IS NEW - check if file is empty
        public bool isReadOnly()
        {
            return this.options.readOnly;
        }

        // FILE OPEN Otvorenie xml súboru
        public bool OpenFile(string FileName)
        {
            if (Os.FileExists(FileName))
            {
                Os.setCurrentDirectory(Os.getFileDirectory(FileName));

                this.CloseFile();
                this.FileName = FileName;
                this.NewFile = false;
                this.SavedFile = true;
                if (new FileInfo(FileName).Length != 0)
                {

                    try
                    {
                        string xml;
                        using (StreamReader streamReader = new StreamReader(FileName, Encoding.UTF8))
                        {
                            xml = streamReader.ReadToEnd();
                        }
                        this.LoadXML(xml);

                    }
                    catch (System.IO.IOException ex)
                    {
                        Program.log.write(ex.Message);
                        MessageBox.Show(main.translations.fileIsLocked);
                        this.CloseFile();
                    }

                    this.SetTitle();

                    return true;
                }
            }

            return false;
        }

        // FILE LOAD XML
        public void LoadXML(string xml)
        {

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            string version = "";
            string salt = "";
            string encrypted = "";

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.Name.ToString() == "version")
                        {
                            version = diagram.Value;
                        }

                        if (diagram.Name.ToString() == "salt")
                        {
                            salt = diagram.Value;
                        }

                        if (diagram.Name.ToString() == "encrypted")
                        {
                            encrypted = diagram.Value;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(main.translations.fileHasWrongFormat);
                Program.log.write("load xml error: " + ex.Message);
                this.CloseFile();
            }

            if (version == "1" && salt != "" && encrypted != "")
            {
                bool error = false;
                do
                {
                    error = false;

                    if (main.passwordForm == null)
                    {
                        main.passwordForm = new PasswordForm(main);
                    }

                    main.passwordForm.Clear();
                    main.passwordForm.ShowDialog();
                    if (!main.passwordForm.cancled)
                    {
                        try
                        {
                            this.password = main.passwordForm.GetPassword();
                            this.salt = Encrypt.SetSalt(salt);
                            this.LoadInnerXML(Encrypt.DecryptStringAES(encrypted, this.password, this.salt));
                        }
                        catch(Exception e)
                        {
                            Program.log.write(e.Message);
                            error = true;
                        }
                    }
                    else
                    {
                        this.CloseFile();
                        return;
                    }

                    main.passwordForm.CloseForm();
                } while (error);
            }
            else
            {
                LoadInnerXML(xml);
            }

        }

        // FILE LOAD XML inner part of diagram file
        public void LoadInnerXML(string xml)
        {
            string FontDefaultString = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(this.FontDefault);

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            List<Node> nodes = new List<Node>();
            List<Line> lines = new List<Line>();

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.HasElements)
                        {

                            if (diagram.Name.ToString() == "option")
                            {
                                foreach (XElement el in diagram.Descendants())
                                {
                                    try
                                    {
                                        if (el.Name.ToString() == "shiftx")
                                        {
                                            this.options.homePosition.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "shifty")
                                        {
                                            this.options.homePosition.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "homelayer")
                                        {
                                            options.homeLayer = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "endPositionx")
                                        {
                                            this.options.endPosition.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "endPositiony")
                                        {
                                            this.options.endPosition.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "startShiftX")
                                        {
                                            options.homePosition.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "startShiftY")
                                        {
                                            options.homePosition.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "diagramreadonly")
                                        {
                                            this.options.readOnly = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "grid")
                                        {
                                            this.options.grid = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "borders")
                                        {
                                            this.options.borders = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "defaultfont")
                                        {
                                            if (el.Attribute("type").Value == "font")
                                            {
                                                this.FontDefault = Fonts.XmlToFont(el);
                                            }
                                            else
                                            {
                                                if (FontDefaultString != el.Value)
                                                {
                                                    this.FontDefault = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(el.Value);
                                                }
                                            }
                                        }

                                        if (el.Name.ToString() == "coordinates")
                                        {
                                            this.options.coordinates = bool.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "firstLayereShift.x")
                                        {
                                            this.options.firstLayereShift.x = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "firstLayereShift.y")
                                        {
                                            this.options.firstLayereShift.y = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.x")
                                        {
                                            this.options.Left = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.y")
                                        {
                                            this.options.Top = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.width")
                                        {
                                            this.options.Width = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.position.height")
                                        {
                                            this.options.Height = Int32.Parse(el.Value);
                                        }

                                        if (el.Name.ToString() == "window.state")
                                        {
                                            this.options.WindowState = Int32.Parse(el.Value);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Program.log.write("load xml diagram options: " + ex.Message);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "rectangles")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {

                                    if (block.Name.ToString() == "rectangle")
                                    {
                                        Node R = new Node();
                                        R.font = this.FontDefault;

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "id")
                                                {
                                                    R.id = Int32.Parse(el.Value);
                                                    maxid = (R.id > maxid) ? R.id : maxid;
                                                }

                                                if (el.Name.ToString() == "font")
                                                {
                                                    if (el.Attribute("type").Value == "font")
                                                    {
                                                        R.font = Fonts.XmlToFont(el);
                                                    }
                                                    else
                                                    {
                                                        if (FontDefaultString != el.Value)
                                                        {
                                                            R.font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(el.Value);
                                                        }
                                                    }
                                                }

                                                if (el.Name.ToString() == "fontcolor")
                                                {
                                                    R.fontcolor = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "text")
                                                {
                                                    R.name = el.Value;
                                                }


                                                if (el.Name.ToString() == "note")
                                                {
                                                    R.note = el.Value;
                                                }


                                                if (el.Name.ToString() == "link")
                                                {
                                                    R.link = el.Value;
                                                }

                                                if (el.Name.ToString() == "scriptid")
                                                {
                                                    R.scriptid = el.Value;
                                                }

                                                if (el.Name.ToString() == "shortcut")
                                                {
                                                    R.shortcut = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "attachment")
                                                {
                                                    R.attachment = el.Value;
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    R.layer = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "haslayer")
                                                {
                                                    R.haslayer = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layershiftx")
                                                {
                                                    R.layerShift.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "layershifty")
                                                {
                                                    R.layerShift.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "x")
                                                {
                                                    R.position.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "y")
                                                {
                                                    R.position.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "width")
                                                {
                                                    R.width = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "height")
                                                {
                                                    R.height = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    R.color = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "transparent")
                                                {
                                                    R.transparent = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "embeddedimage")
                                                {
                                                    R.embeddedimage = bool.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "imagedata")
                                                {
                                                    R.image = new Bitmap(new MemoryStream(Convert.FromBase64String(el.Value)));
                                                    R.height = R.image.Height;
                                                    R.width = R.image.Width;
                                                    R.isimage = true;
                                                }

                                                if (el.Name.ToString() == "image")
                                                {
                                                    R.imagepath = el.Value.ToString();
                                                    if (Os.FileExists(R.imagepath))
                                                    {
                                                        try
                                                        {
                                                            string ext = "";
                                                            ext = Os.getExtension(R.imagepath).ToLower();

                                                            if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp") // skratenie cesty k suboru
                                                            {
                                                                R.image = new Bitmap(R.imagepath);
                                                                if (ext != ".ico") R.image.MakeTransparent(Color.White);
                                                                R.height = R.image.Height;
                                                                R.width = R.image.Width;
                                                                R.isimage = true;
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Program.log.write("load image from xml error: " + ex.Message);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        R.imagepath = "";
                                                    }
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write("load xml nodes error: " + ex.Message);
                                            }
                                        }
                                        nodes.Add(R);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "lines")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {
                                    if (block.Name.ToString() == "line")
                                    {
                                        Line L = new Line();
                                        L.layer = -1; // for identification unset layers

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "start")
                                                {
                                                    L.start = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "end")
                                                {
                                                    L.end = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "arrow")
                                                {
                                                    L.arrow = el.Value == "1" ? true : false;
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    L.color = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    L.layer = Int32.Parse(el.Value);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write("load xml lines error: " + ex.Message);
                                            }
                                        }

                                        lines.Add(L);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(main.translations.fileHasWrongFormat);
                Program.log.write("load xml error: " + ex.Message);
                this.CloseFile();
            }

            int newWidth = 0;
            int newHeight = 0;

            foreach (Node rec in nodes) // Loop through List with foreach
            {
                if (!rec.isimage)
                {
                    SizeF s = rec.measure();
                    newWidth = (int)s.Width;
                    newHeight = (int)s.Height;

                    // font change correction > center node
                    if (rec.width != 0 && newWidth != rec.width) {
                        rec.position.x += (rec.width - newWidth) / 2;
                    }

                    if (rec.height != 0 && newHeight != rec.height) {
                       rec.position.y += (rec.height - newHeight) / 2;
                    }

                    rec.width = newWidth;
                    rec.height = newHeight;
                }

                this.layers.addNode(rec);
            }

            this.layers.buildTree();

            foreach (Line line in lines)
            {

                Line l = this.Connect(
                    this.layers.getNode(line.start),
                    this.layers.getNode(line.end),
                    line.arrow,
                    line.color
                );
            }
        }

        // FILE Save - Ulozit súbor
        public bool save()
        {
            if (this.FileName != "" && Os.FileExists(this.FileName))
            {
                this.SaveXMLFile(this.FileName);
                this.NewFile = false;
                this.SavedFile = true;
                this.SetTitle();

                return true;
            }

            return false;
        }

        // FILE SAVEAS - Uložiť súbor ako
        public void saveas(String FileName)
        {
            this.SaveXMLFile(FileName);
            this.FileName = FileName;
            this.SavedFile = true;
            this.NewFile = false;

            this.SetTitle();
        }

        // FILE SAVE Ulozenie xml súboru
        public void SaveXMLFile(string FileName)
        {
            string diagraxml = "";

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(FileName);
                diagraxml = this.SaveInnerXMLFile();
                if (this.password == "")
                {
                    file.Write(diagraxml);
                }
                else
                {
                    XElement root = new XElement("diagram");
                    try
                    {
                        root.Add(new XElement("version", "1"));
                        this.salt = Encrypt.CreateSalt(14);
                        root.Add(new XElement("encrypted", Encrypt.EncryptStringAES(diagraxml, this.password, this.salt)));
                        root.Add(new XElement("salt", Encrypt.GetSalt(this.salt)));

                        StringBuilder sb = new StringBuilder();
                        XmlWriterSettings xws = new XmlWriterSettings();
                        xws.OmitXmlDeclaration = true;
                        xws.CheckCharacters = false;
                        xws.Indent = true;

                        using (XmlWriter xw = XmlWriter.Create(sb, xws))
                        {
                            root.WriteTo(xw);
                        }

                        file.Write(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        Program.log.write("save file error: " + ex.Message);
                    }
                }

                file.Close();

            }
            catch (System.IO.IOException ex)
            {
                Program.log.write("save file io error: " + ex.Message);
                MessageBox.Show(main.translations.fileIsLocked);
                this.CloseFile();
            }
            catch (Exception ex)
            {
                Program.log.write("save file error: " + ex.Message);
            }
        }

        // FILE SAVE XML
        public string SaveInnerXMLFile()
        {
            bool checkpoint = false;
            XElement root = new XElement("diagram");
            try
            {
                // Options
                XElement option = new XElement("option");
                option.Add(new XElement("shiftx", this.options.homePosition.x));
                option.Add(new XElement("shifty", this.options.homePosition.y));
                option.Add(new XElement("endPositionx", this.options.endPosition.x));
                option.Add(new XElement("endPositiony", this.options.endPosition.y));
                option.Add(new XElement("firstLayereShift.x", this.options.firstLayereShift.x));
                option.Add(new XElement("firstLayereShift.y", this.options.firstLayereShift.y));
                option.Add(new XElement("homelayer", this.options.homeLayer));
                option.Add(new XElement("diagramreadonly", this.options.readOnly));
                option.Add(new XElement("grid", this.options.grid));
                option.Add(new XElement("borders", this.options.borders));
                option.Add(Fonts.FontToXml(this.FontDefault, "defaultfont"));
                option.Add(new XElement("coordinates", this.options.coordinates));
                option.Add(new XElement("window.position.x", this.options.Left));
                option.Add(new XElement("window.position.y", this.options.Top));
                option.Add(new XElement("window.position.width", this.options.Width));
                option.Add(new XElement("window.position.height", this.options.Height));
                option.Add(new XElement("window.state", this.options.WindowState));

                // Rectangles
                XElement rectangles = new XElement("rectangles");
                foreach (Node rec in this.getAllNodes())
                {
                    XElement rectangle = new XElement("rectangle");
                    rectangle.Add(new XElement("id", rec.id));
                    if (!Fonts.compare(this.FontDefault, rec.font))
                    {
                        rectangle.Add(Fonts.FontToXml(rec.font));
                    }
                    rectangle.Add(new XElement("fontcolor", System.Drawing.ColorTranslator.ToHtml(rec.fontcolor)));
                    if (rec.name != "") rectangle.Add(new XElement("text", rec.name));
                    if (rec.note != "") rectangle.Add(new XElement("note", rec.note));
                    if (rec.link != "") rectangle.Add(new XElement("link", rec.link));
                    if (rec.scriptid != "") rectangle.Add(new XElement("scriptid", rec.scriptid));
                    if (rec.shortcut != 0) rectangle.Add(new XElement("shortcut", rec.shortcut));
                    if (rec.attachment != "") rectangle.Add(new XElement("attachment", rec.attachment));

                    rectangle.Add(new XElement("layer", rec.layer));

                    if (rec.haslayer)
                    {
                        rectangle.Add(new XElement("haslayer", rec.haslayer));
                        rectangle.Add(new XElement("layershiftx", rec.layerShift.x));
                        rectangle.Add(new XElement("layershifty", rec.layerShift.y));
                    }

                    rectangle.Add(new XElement("x", rec.position.x));
                    rectangle.Add(new XElement("y", rec.position.y));
                    rectangle.Add(new XElement("width", rec.width));
                    rectangle.Add(new XElement("height", rec.height));
                    rectangle.Add(new XElement("color", System.Drawing.ColorTranslator.ToHtml(rec.color)));
                    if (rec.transparent) rectangle.Add(new XElement("transparent", rec.transparent));
                    if (rec.embeddedimage) rectangle.Add(new XElement("embeddedimage", rec.embeddedimage));

                    if (rec.embeddedimage && rec.image != null) // ak je obrázok vložený priamo do súboru
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                        rectangle.Add(new XElement("imagedata", Convert.ToBase64String((byte[])converter.ConvertTo(rec.image, typeof(byte[])))));
                    }
                    else if (rec.imagepath != "")
                    {
                        rectangle.Add(new XElement("image", rec.imagepath));
                    }

                    rectangle.Add(new XElement("timecreate", rec.timecreate));
                    rectangle.Add(new XElement("timemodify", rec.timemodify));

                    rectangles.Add(rectangle);
                }

                // Lines
                XElement lines = new XElement("lines");
                foreach (Line lin in this.getAllLines())
                {
                    XElement line = new XElement("line");
                    line.Add(new XElement("start", lin.start));
                    line.Add(new XElement("end", lin.end));
                    line.Add(new XElement("arrow", (lin.arrow) ? "1" : "0"));
                    line.Add(new XElement("color", System.Drawing.ColorTranslator.ToHtml(lin.color)));
                    line.Add(new XElement("layer", lin.layer));
                    lines.Add(line);
                }

                root.Add(option);
                root.Add(rectangles);
                root.Add(lines);

                checkpoint = true;
            }
            catch (Exception ex)
            {
                Program.log.write("save xml error: " + ex.Message);
            }

            if (checkpoint)
            {
                try
                {

                    StringBuilder sb = new StringBuilder();
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.CheckCharacters = false;
                    xws.Indent = true;

                    using (XmlWriter xw = XmlWriter.Create(sb, xws))
                    {
                        root.WriteTo(xw);
                    }

                    return sb.ToString();

                }
                catch (Exception ex)
                {
                    Program.log.write("write xml to file error: " + ex.Message);
                }

            }

            return "";
        }

        // FILE UNSAVE Subor sa zmenil treba ho ulozit
        public void unsave()
        {
            this.SavedFile = false;
            this.SetTitle();
        }

        // FILE CLOSE - Vycisti  nastavenie do  východzieho tavu a prekresli obrazovku
        public void CloseFile()
        {
            // Prednadstavenie atributov
            this.maxid = 0;
            this.NewFile = true;
            this.SavedFile = true;
            this.FileName = "";

            // clear nodes and lines lists
            this.layers.clear();

            this.options.readOnly = false;
            this.options.grid = true;
            this.options.coordinates = false;

            this.TextWindows.Clear();
        }

        /*************************************************************************************************************************/

        public List<Node> getAllNodes()
        {
            return this.layers.getAllNodes();
        }

        public List<Line> getAllLines()
        {
            return this.layers.getAllLines();
        }

        // NODE Najdenie nody podla id
        public Node GetNodeByID(int id)
        {
            return this.layers.getNode(id);
        }

        // NODE Najdenie nody podla scriptid
        public Node getNodeByScriptID(string id)
        {
            foreach (Node rec in this.getAllNodes()) // Loop through List with foreach
            {
                if (rec.scriptid == id) return rec;
            }

            return null;
        }

        // NODE delete all nodes which is not in layer history
        public bool canDeleteNode(Node rec)
        {
            if (!rec.haslayer)
            {
                foreach (DiagramView view in this.DiagramViews)
                {
                    if (view.isNodeInLayerHistory(rec))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // NODE Zmazanie nody
        public void DeleteNode(Node rec)
        {
            if (rec != null && !this.options.readOnly)
            {
                foreach (DiagramView DiagramView in this.DiagramViews) //remove node from selected nodes in views
                {
                    if (DiagramView.selectedNodes.Count() > 0)
                    {
                        for (int i = DiagramView.selectedNodes.Count() - 1; i >= 0; i--)
                        {
                            if (DiagramView.selectedNodes[i] == rec)
                            {
                                DiagramView.selectedNodes.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                if (this.TextWindows.Count() > 0) // close text edit to node
                {
                    for (int i = this.TextWindows.Count() - 1; i >= 0; i--)
                    {
                        if (this.TextWindows[i].rec == rec)
                        {
                            this.TextWindows[i].Close();
                            break;
                        }
                    }
                }

                this.layers.removeNode(rec);
                this.unsave();
            }
        }

        // NODE Editovanie vlastnosti nody
        public TextForm EditNode(Node rec)
        {
            bool found = false;
            for (int i = TextWindows.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (TextWindows[i].rec == rec)
                {
                    TextWindows[i].SetFocus();
                    TextWindows[i].Focus();
                    found = true;
                    return TextWindows[i];
                }
            }

            if (!found) {
                TextForm textf = new TextForm(main);
                textf.setDiagram(this);
                textf.rec = rec;
                string[] lines = rec.name.Split(Environment.NewLine.ToCharArray()).ToArray();
                if(lines.Count()>0)
                    textf.Text = lines[0];

                this.TextWindows.Add(textf);
                main.TextWindows.Add(textf);
                textf.Show();
                textf.SetFocus();
                return textf;
            }
            return null;
        }

        // NODE Editovanie vlastnosti nody
        public void EditNodeClose(Node rec)
        {
            for (int i = TextWindows.Count() - 1; i >= 0; i--) // Loop through List with foreach
            {
                if (TextWindows[i].rec == rec)
                {
                    TextWindows.RemoveAt(i);
                }
            }
        }

        // NODE Create Rectangle on point
        public Node createNode(Position position, string name = "", int layer = 0, Color? color = null, Font font = null, Layer parentLayer = null)
        {
            if (!this.options.readOnly)
            {
                var rec = new Node();
                if (font == null)
                {
                    rec.font = this.FontDefault;
                }
                else
                {
                    rec.font = font;
                }

                rec.id = ++maxid;
                rec.layer = layer;

                rec.setName(name);
                rec.note = "";
                rec.link = "";

                rec.position.set(position);

                rec.color = color ?? Media.getColor(this.options.colorNode);

                DateTime dt = DateTime.Now;
                rec.timecreate = String.Format("{0:yyyy-M-d HH:mm:ss}", dt);
                rec.timemodify = rec.timecreate;

                this.layers.addNode(rec);

                return rec;
            }

            return null;
        }

        // NODE CONNECT connect two nodes
        public Line Connect(Node a, Node b, int layer = 0)
        {
            if (!this.options.readOnly && a != null && b != null)
            {
                Line line = this.layers.getLine(a, b);

                if (line == null)
                {
                    line = new Line();
                    line.start = a.id;
                    line.end = b.id;
                    line.startNode = this.GetNodeByID(line.start);
                    line.endNode = this.GetNodeByID(line.end);
                    line.layer = layer;
                    this.layers.addLine(line);

                    return line;
                }
                else
                {
                    this.layers.removeLine(line);
                }
            }

            return null;
        }

        public Line Connect(Node a, Node b, bool arrow = false, Color? color = null)
        {
            int layer = 0;

            if (a.layer == b.layer)
            {
                layer = a.layer;
            }
            else
            if (a.layer == b.id)
            {
                layer = a.layer;
            }
            else
            if (b.layer == a.id)
            {
                layer = b.layer;
            }

            Line line = this.Connect(a, b, layer);

            if (line != null)
            {
                line.arrow = arrow;
                line.color = color ?? Color.Black;
            }

            return line;
        }

        // NODES ALIGN to column
        public void AlignToColumn(List<Node> Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int miny = Nodes[0].position.y;
                int topx = Nodes[0].position.x;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.y <= miny)
                    {
                        miny = rec.position.y;
                        topx = rec.position.x + rec.width / 2; ;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.x = topx - rec.width / 2;
                }
            }
        }

        // NODES ALIGN to line
        public void AlignToLine(List<Node> Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int minx = Nodes[0].position.x;
                int topy = Nodes[0].position.y;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x <= minx)
                    {
                        minx = rec.position.x;
                        topy = rec.position.y + rec.height / 2;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.y = topy - rec.height / 2;
                }
            }
        }

        // NODES ALIGN compact
        public void AlignCompact(List<Node> Nodes)
        {
            // vyratanie ci je mensi profil na sirku alebo na vysku a potom ich zarovnat
            // po zarovnani by sa nemali prekrivat
            // ked su zarovnané pozmensovat medzery medzi nimi na nejaku konstantnu vzdialenost

            if (Nodes.Count() > 0)
            {
                int minx = Nodes[0].position.x;
                int miny = Nodes[0].position.y;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x <= minx) // find top left element
                    {
                        minx = rec.position.x;
                    }

                    if (rec.position.y <= miny) // find most top element
                    {
                        miny = rec.position.y;
                    }
                }

                foreach (Node rec in Nodes) // align to left
                {
                    rec.position.x = minx;
                }

                // sort elements by y coordinate
                List<Node> SortedList = Nodes.OrderBy(o => o.position.y).ToList();

                int posy = miny;
                foreach (Node rec in SortedList) // zmensit medzeru medzi objektami
                {
                    rec.position.y = posy;
                    posy = posy + rec.height + 10;
                }
            }
        }

        // NODES ALIGN left
        public void AlignRight(List<Node> Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int maxx = Nodes[0].position.x + Nodes[0].width;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x + rec.width >= maxx)
                    {
                        maxx = rec.position.x + rec.width;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.x = maxx - rec.width;
                }
                this.unsave();
                this.InvalidateDiagram();
            }
        }

        // NODES ALIGN left
        public void AlignLeft(List<Node> Nodes)
        {
            if (Nodes.Count() > 0)
            {
                int minx = Nodes[0].position.x;
                foreach (Node rec in Nodes)
                {
                    if (rec.position.x <= minx)
                    {
                        minx = rec.position.x;
                    }
                }

                foreach (Node rec in Nodes)
                {
                    rec.position.x = minx;
                }
            }
        }

        // NODES remove shortcut
        public void RemoveShortcut(Node node)
        {
            if (node.shortcut > 0) node.shortcut = 0;
        }

        // NODE Reset font to default font for all nodes
        public void ResetFont()
        {
            foreach (Node rec in this.getAllNodes()) // Loop through List with foreach
            {
                rec.font = this.FontDefault;
                rec.resize();
            }

            this.unsave();
            this.InvalidateDiagram();
        }

        // NODE Reset font to default font for group of nodes
        public void ResetFont(List<Node> Nodes)
        {
            if (Nodes.Count>0) {
                foreach (Node rec in Nodes) // Loop through List with foreach
                {
                    rec.font = this.FontDefault;
                    rec.resize();
                }
                this.unsave();
                this.InvalidateDiagram();
            }
        }

        // NODE Najdenie nody podla pozicie myši
        public Node findNodeInPosition(Position position, int layer)
        {
            foreach (Node node in this.layers.getLayer(layer).nodes) // Loop through List with foreach
            {
                if (layer == node.layer || layer == node.id)
                {
                    if
                    (
                        node.position.x <= position.x && position.x <= node.position.x + node.width &&
                        node.position.y <= position.y && position.y <= node.position.y + node.height
                    )
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        // NODE set image
        public void setImage(Node rec, string file)
        {
            string ext = Os.getExtension(file);

            rec.isimage = true;
            rec.imagepath = file;
            if (this.FileName != ""
                && Os.FileExists(this.FileName)
                && file.IndexOf(new FileInfo(this.FileName).DirectoryName) == 0)
            {
                int start = new FileInfo(this.FileName).DirectoryName.Length;
                int finish = file.Length - start;
                rec.imagepath = "." + file.Substring(start, finish);
            }
            rec.image = new Bitmap(rec.imagepath);
            if (ext != ".ico") rec.image.MakeTransparent(Color.White);
            rec.height = rec.image.Height;
            rec.width = rec.image.Width;
        }

        // NODE remove image
        public void removeImage(Node rec)
        {
            rec.isimage = false;
            rec.imagepath = "";
            rec.image = null;
            rec.embeddedimage = false;
            rec.resize();
        }

        // NODE set image embedded
        public void setImageEmbedded(Node rec)
        {
            if (rec.isimage)
            {
                rec.embeddedimage = true;
            }
        }

        /*************************************************************************************************************************/

        // LAYER MOVE posunie rekurzivne layer a jeho nody OBSOLATE
        public void MoveLayer(Node rec, Position vector)
        {
            if (rec != null)
            {
                List<Node> nodes = this.layers.getLayer(rec.id).nodes;
                foreach (Node node in nodes) // Loop through List with foreach
                {
                    if (node.layer == rec.id)
                    {
                        node.position.add(vector);

                        if (node.haslayer)
                        {
                            MoveLayer(node, vector);
                        }
                    }
                }
            }
        }

        /*************************************************************************************************************************/

        // DIAGRAM VIEW open new view on diagram
        public void openDiagramView()
        {
            DiagramView diagramview = new DiagramView(main, this);
            diagramview.setDiagram(this);
            this.DiagramViews.Add(diagramview);
            main.DiagramViews.Add(diagramview);
			this.SetTitle();
            diagramview.Show();
        }

        // DIAGRAM VIEW invalidate all opened views
        public void InvalidateDiagram()
        {
            foreach (DiagramView DiagramView in this.DiagramViews)
            {
                if (DiagramView.Visible == true)
                {
                    DiagramView.Invalidate();
                }
            }
        }

        // DIAGRAM close diagram
        public void CloseDiagram()
        {
            bool canclose = true;

            if (this.DiagramViews.Count > 0 || TextWindows.Count > 0)
            {
                canclose = false;
            }

            if (canclose)
            {
                this.CloseFile();
                main.Diagrams.Remove(this);
                main.CloseEmptyApplication();
            }
        }

        // DIAGRAM VIEWS change title
        public void SetTitle()
        {
            foreach (DiagramView DiagramView in this.DiagramViews)
            {
                DiagramView.SetTitle();
            }
        }

        /*************************************************************************************************************************/

        // CLIPBOARD PASTE paste part of diagram from clipboard                                   // CLIPBOARD
        public List<Node> AddDiagramPart(string DiagramXml, Position position, int layer)
        {
            List<Node> NewNodes = new List<Node>();
            List<Line> NewLines = new List<Line>();

            XmlReaderSettings xws = new XmlReaderSettings();
            xws.CheckCharacters = false;

            string xml = DiagramXml;

            try
            {
                using (XmlReader xr = XmlReader.Create(new StringReader(xml), xws))
                {

                    XElement root = XElement.Load(xr);
                    foreach (XElement diagram in root.Elements())
                    {
                        if (diagram.HasElements)
                        {

                            if (diagram.Name.ToString() == "rectangles")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {

                                    if (block.Name.ToString() == "rectangle")
                                    {
                                        Node R = new Node();
                                        R.font = this.FontDefault;

                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "id")
                                                {
                                                    R.id = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "text")
                                                {
                                                    R.name = el.Value;
                                                }


                                                if (el.Name.ToString() == "note")
                                                {
                                                    R.note = el.Value;
                                                }

                                                if (el.Name.ToString() == "x")
                                                {
                                                    R.position.x = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "y")
                                                {
                                                    R.position.y = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    R.color = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }


                                                if (el.Name.ToString() == "font")
                                                {
                                                    R.font = Fonts.XmlToFont(el);
                                                }


                                                if (el.Name.ToString() == "fontcolor")
                                                {
                                                    R.fontcolor = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "link")
                                                {
                                                    R.link = el.Value;
                                                }

                                                if (el.Name.ToString() == "shortcut")
                                                {
                                                    R.shortcut = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "transparent")
                                                {
                                                    R.transparent = bool.Parse(el.Value);
                                                }


                                                if (el.Name.ToString() == "timecreate")
                                                {
                                                    R.timecreate = el.Value;
                                                }


                                                if (el.Name.ToString() == "timemodify")
                                                {
                                                    R.timemodify = el.Value;
                                                }

                                                if (el.Name.ToString() == "attachment")
                                                {
                                                    R.attachment = el.Value;
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    R.layer = Int32.Parse(el.Value);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
                                            }
                                        }

                                        NewNodes.Add(R);
                                    }
                                }
                            }

                            if (diagram.Name.ToString() == "lines")
                            {
                                foreach (XElement block in diagram.Descendants())
                                {
                                    if (block.Name.ToString() == "line")
                                    {
                                        Line L = new Line();
                                        foreach (XElement el in block.Descendants())
                                        {
                                            try
                                            {
                                                if (el.Name.ToString() == "start")
                                                {
                                                    L.start = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "end")
                                                {
                                                    L.end = Int32.Parse(el.Value);
                                                }

                                                if (el.Name.ToString() == "arrow")
                                                {
                                                    L.arrow = el.Value == "1" ? true : false;
                                                }

                                                if (el.Name.ToString() == "color")
                                                {
                                                    L.color = System.Drawing.ColorTranslator.FromHtml(el.Value.ToString());
                                                }

                                                if (el.Name.ToString() == "layer")
                                                {
                                                    L.layer = Int32.Parse(el.Value);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
                                            }
                                        }
                                        NewLines.Add(L);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.log.write(main.translations.dataHasWrongStructure + ": error: " + ex.Message);
            }


            List<Node[]> maps = new List<Node[]>();

            List<Node> NewReorderedNodes = new List<Node>();
            this.nodesReorderNodes(0, null, NewNodes, NewReorderedNodes);

            int layerParent = 0;

            foreach (Node rec in NewReorderedNodes)
            {
                layerParent = 0;
                if (rec.layer == 0)
                {
                    layerParent = layer;
                }
                else
                { 
                    foreach (Node[] mapednode in maps)
                    {
                        if (rec.layer == mapednode[0].id)
                        {
                            layerParent = mapednode[1].id;
                            break;
                        }
                    }
                }

                Node newrec = this.createNode(
                    rec.position.clone().add(position),
                    rec.name,
                    layerParent,
                    null,
                    rec.font
                );

                newrec.note = rec.note;
                newrec.color = rec.color;
                newrec.fontcolor = rec.fontcolor;
                newrec.link = rec.link;
                newrec.shortcut = rec.shortcut;
                newrec.transparent = rec.transparent;
                newrec.timecreate = rec.timecreate;
                newrec.timemodify = rec.timemodify;

                maps.Add(new Node[2] { rec, newrec });
            }

            // fix layers and shortcuts
            foreach (Node rec in NewNodes)
            {
                if (rec.shortcut != 0)
                { 
                    foreach (Node[] mapednode in maps)
                    {
                        if (rec.shortcut == mapednode[0].id)
                        {
                            rec.shortcut = mapednode[1].id;
                            break;
                        }
                    }
                }
            }

            foreach (Line line in NewLines)
            {
                foreach (Node[] mapbegin in maps)
                {
                    if (line.start == mapbegin[0].id)
                    {
                        foreach (Node[] mapend in maps)
                        {
                            if (line.end == mapend[0].id)
                            {
                                this.Connect(
                                    mapbegin[1],
                                    mapend[1],
                                    line.arrow,
                                    line.color
                                );
                            }
                        }
                    }
                }
            }

            return NewNodes;
        }

        // CLIPBOARD Get all layers nodes
        private void nodesReorderNodes(int layer, Node parent, List<Node> nodesIn, List<Node> nodesOut)
        {
            foreach (Node node in nodesIn)
            {
                if (node.layer == layer)
                {
                    if (parent != null) {
                        parent.haslayer = true;
                    }

                    nodesOut.Add(node);

                    nodesReorderNodes(node.id, node, nodesIn, nodesOut);
                }
            }
        }

        // CLIPBOARD Get all layers nodes
        public void getLayerNodes(Node node, List<Node> nodes)
        {
            if (node.haslayer) {
                foreach(Node subnode in this.layers.getLayer(node.id).nodes) {
                    nodes.Add(subnode);

                    if (subnode.haslayer) {
                        getLayerNodes(subnode, nodes);
                    }
                }
            }
        }

        // CLIPBOARD COPY copy part of diagram to text xml string
        public string GetDiagramPart(List<Node> nodes)
        {
            string copyxml = "";

            if (nodes.Count() > 0)
            {
                XElement root = new XElement("diagram");
                XElement rectangles = new XElement("rectangles");
                XElement lines = new XElement("lines");

                int minx = nodes[0].position.x;
                int miny = nodes[0].position.y;
                int minid = nodes[0].id;

                List<Node> subnodes = new List<Node>();

                foreach (Node node in nodes)
                {
                    getLayerNodes(node, subnodes);
                }

                foreach (Node node in subnodes)
                {
                    nodes.Add(node);
                }

                foreach (Node node in nodes)
                {
                    if (node.position.x < minx) minx = node.position.x;
                    if (node.position.y < miny) miny = node.position.y;
                    if (node.id < minid) minid = node.id;
                }

                foreach (Node rec in nodes)
                {
                    XElement rectangle = new XElement("rectangle");
                    rectangle.Add(new XElement("id", rec.id - minid + 1));
                    rectangle.Add(new XElement("x", rec.position.x - minx));
                    rectangle.Add(new XElement("y", rec.position.y - miny));
                    rectangle.Add(new XElement("text", rec.name));
                    rectangle.Add(new XElement("note", rec.note));
                    rectangle.Add(new XElement("color", System.Drawing.ColorTranslator.ToHtml(rec.color)));
                    rectangle.Add(Fonts.FontToXml(rec.font));
                    rectangle.Add(new XElement("fontcolor", System.Drawing.ColorTranslator.ToHtml(rec.fontcolor)));
                    if (rec.link != "") rectangle.Add(new XElement("link", rec.link));
                    if (rec.shortcut != 0 && rec.shortcut - minid + 1 > 0) rectangle.Add(new XElement("shortcut", rec.shortcut + 1));
                    rectangle.Add(new XElement("transparent", rec.transparent));
                    rectangle.Add(new XElement("timecreate", rec.timecreate));
                    rectangle.Add(new XElement("timemodify", rec.timemodify));
                    rectangle.Add(new XElement("attachment", rec.attachment));
                    if (rec.layer != 0 && rec.layer - minid + 1 > 0)  rectangle.Add(new XElement("layer", rec.layer - minid + 1));

                    rectangles.Add(rectangle);
                }

                foreach (Line li in this.getAllLines())
                {
                    foreach (Node recstart in nodes)
                    {
                        if (li.start == recstart.id)
                        {
                            foreach (Node recend in nodes)
                            {
                                if (li.end == recend.id)
                                {
                                    XElement line = new XElement("line");
                                    line.Add(new XElement("start", li.start - minid + 1));
                                    line.Add(new XElement("end", li.end - minid + 1));
                                    line.Add(new XElement("arrow", (li.arrow) ? "1" : "0"));
                                    line.Add(new XElement("color", System.Drawing.ColorTranslator.ToHtml(li.color)));
                                    if (li.layer - minid +1 > 0) {
                                        line.Add(new XElement("layer", li.layer - minid + 1));
                                    }
                                    lines.Add(line);
                                }

                            }
                        }
                    }
                }

                root.Add(rectangles);
                root.Add(lines);
                copyxml = root.ToString();
            }

            return copyxml;
        }
    }
}
