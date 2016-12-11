﻿using System;
using System.Drawing;

namespace Diagram
{
    /// <summary>
    /// Node in diagram</summary>
    public class Node
    {
        public int id = 0; // node unique id

        /*************************************************************************************************************************/
        // SIZE AND POSITION

        public Position position = new Position(); // node position in canvas
        public int width = 0; // node size counted from current font
        public int height = 0;

        /*************************************************************************************************************************/
        // FLAGS

        public bool selected = false; // node is selected by mouse
        public bool visible = true;

        /*************************************************************************************************************************/
        // STYLES

        public ColorType color = new ColorType("#FFFFB8"); // node color
        public Font font = null; // node name font
        public ColorType fontcolor = new ColorType(); // node name ext color
        public bool transparent = false; // node is transparent, color is turn off

        /*************************************************************************************************************************/
        // TEXT

        public string name = ""; // node name
        public string note = ""; // node note
        public string link = ""; // node link to external source

        /*************************************************************************************************************************/
        // LAYER

        public int layer = 0; // layer id or parent node id
        public bool haslayer = false; // nose has one or more childrens
        public Position layerShift = new Position(); // last position in layer

        /*************************************************************************************************************************/
        // SHORTCUT

        public int shortcut = 0; // node id whitch is linked with this node
        public bool mark = false; // mark node position for navigation history

        /*************************************************************************************************************************/
        // IMAGE

        public bool isimage = false; // show node as image instead of text
        public bool embeddedimage = false; // image is imported to xml file as string
        public string imagepath = ""; // path to node image
        public Bitmap image = null; // loaded image
        public int iwidth = 0; //image size
        public int iheight = 0;

        /*************************************************************************************************************************/
        // ATTACHMENT

        public string attachment = ""; // compressed file attachment

        /*************************************************************************************************************************/
        // TIME

        public string timecreate = ""; // node creation time
        public string timemodify = ""; // node modification time

        /*************************************************************************************************************************/
        // SCRIPT

        public string scriptid = ""; // node text id for in script search

        /*************************************************************************************************************************/
        // PADDING

        public const int NodePadding = 10;             // node padding around node name text
        public const int EmptyNodePadding = 20;        // node padding for empty node circle
        public const string protectedName = "*****";   // name for protected node
        
        /*************************************************************************************************************************/
        // SECURITY

        public bool protect = false; // protect sensitive data like pasword in node name (show asterisk instead of name)

        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public Node()
        {
        }

        public Node(Node node)
        {
            this.set(node);
        }

        /*************************************************************************************************************************/
        // SETTERS AND GETTERS

        public void set(Node node)
        {
            this.id = node.id;

            this.color.set(node.color);
            this.font = node.font;
            this.fontcolor.set(node.fontcolor);
            this.transparent = node.transparent;

            this.name = node.name;
            this.note = node.note;
            this.link = node.link;

            this.position.Set(node.position);
            this.width = node.width;
            this.height = node.height;

            this.layer = node.layer;
            this.haslayer = node.haslayer;
            this.layerShift = node.layerShift;

            this.shortcut = node.shortcut;
            this.mark = node.mark;

            this.isimage = node.isimage;
            this.embeddedimage = node.embeddedimage;
            this.imagepath = node.imagepath;
            this.image = node.image;

            this.iwidth = node.iwidth;
            this.iheight = node.iheight;

            this.attachment = node.attachment;

            this.timecreate = node.timecreate;
            this.timemodify = node.timemodify;

            this.scriptid = node.scriptid;

            this.protect = node.protect;
        }

        /// <summary>
        /// node copy from another node to current node</summary>
        public void copyNode(Node node, bool skipPosition = false, bool skipSize = false) 
        {
            this.color.set(node.color);
            this.font = node.font;
            this.fontcolor.set(node.fontcolor);
            this.transparent = node.transparent;

            this.name = node.name;
            this.note = node.note;
            this.link = node.link;

            this.shortcut = node.shortcut;
            this.mark = node.mark;

            if (!skipPosition)
            {
                this.position.Set(node.position);
            }

            if (!skipSize)
            {
                this.width = node.width;
                this.height = node.height;
            }

            this.isimage = node.isimage;
            this.embeddedimage = node.embeddedimage;
            this.imagepath = node.imagepath;
            this.image = node.image;

            this.iwidth = node.iwidth;
            this.iheight = node.iheight;

            this.timecreate = node.timecreate;
            this.timemodify = node.timemodify;

            this.scriptid = node.scriptid;

            this.protect = node.protect;
        }

        /// <summary>
        /// node copy style from another node to current node</summary>
        public void copyNodeStyle(Node node)
        {
            this.color.set(node.color);
            this.font = node.font;
            this.fontcolor.set(node.fontcolor);
            this.transparent = node.transparent;

            this.isimage = node.isimage;
            this.embeddedimage = node.embeddedimage;
            this.imagepath = node.imagepath;
            this.image = node.image;

            this.iwidth = node.iwidth;
            this.iheight = node.iheight;

            this.protect = node.protect;

            this.resize();
        }

        /// <summary>
        /// clone node to new node</summary>
        public Node clone()
        {
            return new Node(this);
        }

        /*************************************************************************************************************************/
        // RESIZE

        public void setName(string name)
        {
            this.name = name;

            if (this.protect)
            {
                this.resizeProtect();
            }
            else
            {
                this.resize();
            }
        }

        public SizeF measure()
        {
            SizeF s;

            if (name != "")
            {
                s = Fonts.MeasureString((this.protect) ? Node.protectedName : this.name, this.font);
                s.Height += 2 * Node.NodePadding;
                s.Width += 2 * Node.NodePadding;
            }
            else
            {
                s = new SizeF(Node.EmptyNodePadding, Node.EmptyNodePadding);
            }

            return s;
        }

        public void resize()
        {
            if (!this.isimage)
            {
                if (!this.protect)
                {
                    SizeF s = measure();

                    this.width = (int)s.Width;
                    this.height = (int)s.Height;
                }
                else
                {
                    this.resizeProtect();
                }
            }
        }

        public void resizeProtect()
        {
            SizeF s = Fonts.MeasureString(Node.protectedName, this.font);
            s.Height += 2 * Node.NodePadding;
            s.Width += 2 * Node.NodePadding;

            this.width = (int)s.Width;
            this.height = (int)s.Height;
        }

        public void setProtect(bool protect)
        {
            this.protect = protect;

            if (this.protect)
            {
                this.resizeProtect();
            }
            else
            {
                this.resize();
            }
        }

        /*************************************************************************************************************************/
        // IMAGE

        public void loadImage()
        {
            if (this.imagepath != "" && Os.FileExists(this.imagepath))
            {
                try
                {
                    string ext = "";
                    ext = Os.GetExtension(this.imagepath).ToLower();

                    if (ext == ".jpg" || ext == ".png" || ext == ".ico" || ext == ".bmp")
                    {
                        this.image = Media.getImage(this.imagepath);
                        if (ext != ".ico") this.image.MakeTransparent(Color.White);
                        this.height = this.image.Height;
                        this.width = this.image.Width;
                        this.isimage = true;
                    }
                }
                catch (Exception ex)
                {
                    Program.log.write("load image from xml error: " + ex.Message);
                }
            }
            else
            {
                this.imagepath = "";
            }
        }

        /*************************************************************************************************************************/
        // SEARCH

        /// <summary>
        /// Check if node contain string in some attribute (for search nodes by string)</summary>
        public bool Contain(string searchFor)
        {
            if (name.Contains(searchFor))
            {
                return true;
            }

            if (note.Contains(searchFor))
            {
                return true;
            }

            if (link.Contains(searchFor))
            {
                return true;
            }

            return false;
        }

    }
}
