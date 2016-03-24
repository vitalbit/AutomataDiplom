using System;
//
using System.Collections;
using System.Collections.Generic;
//
using System.Drawing;

namespace InputPostfixRegex
{
    /*
     Id = <a><a><9><Join><Star><Concat>
     * 
     Oct = <0><0><7><Join><Star><Concat><><ul><Join><lu><Join><u><Join><l><Join><Concat>
     */
    public class TreeChart
    {

        //>>>>>>>>> for View
        public const float emSizeChar = 10.0f; //Using >=8      
        public const int kCHAR = 3; //Using > 2      


        public const float penEdgeWidth = 1.0f;//Using

        public const int radiusVertex = 5;//Using

        public const int widthArrow = 11;//Using
        public const int heightArrow = 4;//Using >=4

        public const int up_down_Star = 4 + heightArrow;//Using == heightArrow
        public const int baseElement = 2;
        //<<<<<<<<<<<<<<<<< for View


        public Graphics g; //Using      //static for View

        public string DiagramName; // valid for ROOT of tree; for subtree is null
        ///////////////////////

        public string element; //Using
        public Size rectSize;  //Using
        public TreeChart[] subTrees; //Using

        public string RegExp; //for Root Valid only: optimized Result?

        public ExprNameArray namedPostfixExp; //for Root Valid only: Source?


        //Using
        public TreeChart(string element, Size rectSize, TreeChart[] subTrees)
        {
            this.element = element;
            this.rectSize = rectSize;
            this.subTrees = subTrees;
            this.RegExp = null;
        }

        public static string PolishListToString(string[] polishList)//List<string>
        {
            string res = "";
            if (polishList == null)
                return "";
            foreach (string token in polishList)
                if (token == "")
                    res = res + ' ' + "\"\"";
                else switch (token)
                    {
                        case "Star":
                            res = res + ' ' + "^";
                            break;
                        case "Join":
                            res = res + ' ' + "|";
                            break;
                        case "Concat":
                            res = res + ' ' + "*";
                            break;
                        default:

                            res = res + ' ' + token;
                            break;
                    }


            return res;
        }

        //USING Pen> penEdgeWidth,(kCHAR - 1) * (int)emSizeChar, baseElement and g
        public void DrawThisElementAt(Point point)
        {


            Point startElement = new Point(point.X, point.Y);// + root.rectSize.Height/2);
            startElement.Offset(0, this.rectSize.Height / 2);
            Point endElement = new Point(point.X, point.Y);
            endElement.Offset(this.rectSize.Width, this.rectSize.Height / 2);

            //<<startElement>> is 0-vertex and <<endElement>> is 1-vertex for an EDGE (0,"s",1)            

            //Draw 0---1 Line 
            Pen penEdge = new Pen(Color.Blue, penEdgeWidth);
            g.DrawLine(penEdge, startElement, endElement);

            //Draw Symbol "a" or "\"" over 0---1 Line

            //tree.rectSize = new Size((kCHAR - 1) * (int)emSizeChar + tree.element.Length * (int)emSizeChar + (kCHAR + 1) * (int)emSizeChar,
            //                       2 * (4 + (int)emSizeChar + baseElement));//+ 4



            Point pointLeftUpRect = new Point(startElement.X + (kCHAR - 1) * (int)emSizeChar,
                startElement.Y - (4 + (int)emSizeChar + baseElement));

            drawSymbol_Element(pointLeftUpRect, this.element);
            //drawSymbol_OLD(point, this.element);

            //Draw 0-Vertex
#if DRAW_ADDITIONAL_VERTEX
            drawVertex(startElement);
#endif

            //Draw 1-Vertex
#if DRAW_ADDITIONAL_VERTEX
            drawVertex(endElement);
//#endif
           
            //Draw Arrow at 1-Vertex
            drawArrow(new Point(endElement.X - radiusVertex, endElement.Y));           
#endif
        }


        //USING kCHAR * (int)emSizeChar, Pen > penEdgeWidth, radiusVertex,nVertex, up_down_Star and g
        public void DrawThisStarAt(Point point, ref int nVertex)
        {
            Point startStar = new Point(point.X, point.Y);// + root.rectSize.Height/2);
            startStar.Offset(0, this.rectSize.Height / 2);
            Point endStar = new Point(point.X, point.Y);
            endStar.Offset(this.rectSize.Width, this.rectSize.Height / 2);

            //Size starSize 
            //tree.rectSize = new Size(kCHAR * (int)emSizeChar + arg1Tree.rectSize.Width + kCHAR * (int)emSizeChar,
            //        up_down_Star + arg1Tree.rectSize.Height + up_down_Star);

            //1)Draw Line 0->1 Before this.subTrees[0] !!!
            Point[] B = new Point[2];
            B[0] = new Point(startStar.X, startStar.Y);//+V for this
            B[1] = new Point(startStar.X + kCHAR * (int)emSizeChar, startStar.Y);//+Arrow

            Pen penEdge = new Pen(Color.Blue, penEdgeWidth);
            g.DrawLine(penEdge, B[0], B[1]);

            //2)Skip Drawing  Start Vertex B[0] !!!

#if DRAW_ADDITIONAL_VERTEX
            drawVertex(startStar);
#endif
            //2)Not Skip Drawing  Vertex B[1] !!!
            drawVertex(B[1], Brushes.Black);

            //3)Draw Arrow at Vertex B[1] !!!            
            drawArrow(new Point(B[1].X - radiusVertex, B[1].Y));


            //Point B1_Right = new Point(B[1].X + 2 * radiusVertex, B[1].Y);//Shift to Right on 2 * radiusVertex from B[1]!!!
            int dLeft = (int)(emSizeChar * nVertex.ToString().Length);
            Point B1_Left = new Point(B[1].X - dLeft, B[1].Y);

            drawNumberVertex(B1_Left, nVertex);
            //ENUMERATE GLOBAL <<nVertex>>
            nVertex++; //Count True Vertex

            //2.1)Draw Line n->n+1 After this.subTrees[0] !!!

            Point[] A = new Point[2];
            A[0] = new Point(endStar.X - kCHAR * (int)emSizeChar, endStar.Y);//
            A[1] = new Point(endStar.X, endStar.Y);// +arrow

            //Pen penEdge = new Pen(Color.Blue, penEdgeWidth);
            g.DrawLine(penEdge, A[0], A[1]);

            //2.2)Skip Drawing End Vertex A[1] !!!
#if DRAW_ADDITIONAL_VERTEX
            drawVertex(endStar);
#endif
            //2.2)NOT Skip Drawing Vertex A[0] !!!
            drawVertex(A[0], Brushes.Black);

            //2.3)Draw Arrow at Vertex A[1] !!!

            drawArrow(new Point(A[0].X - radiusVertex, A[0].Y));

            //int dLeft = (int)(emSizeChar * nVertex.ToString().Length);
            //Point A0_Left = new Point(A[0].X - dLeft, A[0].Y);
            //drawNumberVertex(A0_Left, nVertex);

            Point A0_Right = new Point(A[0].X, A[0].Y);

            drawNumberVertex(A0_Right, nVertex);
            //ENUMERATE GLOBAL <<nVertex>>
            nVertex++;//Count True Vertex

            //3)Draw Lines-Frame-Ellipse  !!!
            Rectangle frameEllipse = new Rectangle();
            frameEllipse.X = B[1].X;    // point.X + kCHAR * (int)emSizeChar;
            frameEllipse.Y = point.Y + up_down_Star / 2; //heightArrow

            frameEllipse.Width = this.rectSize.Width - 2 * kCHAR * (int)emSizeChar;
            frameEllipse.Height = this.rectSize.Height - up_down_Star; //heightArrow

            g.DrawEllipse(penEdge, frameEllipse);

            //Draw hollow ARROW between Up frameEllipse !!!
            Point betweenUpEllipse = new Point((startStar.X + endStar.X) / 2, frameEllipse.Y);
            drawHollowArrow(betweenUpEllipse);

            //Draw hollow inverse ARROW between Down frameEllipse !!!
            Point betweenDownEllipse = new Point((startStar.X + endStar.X) / 2, frameEllipse.Y + frameEllipse.Height);
            drawHollowInverseArrow(betweenDownEllipse);

        }

        //USING nothing, SEE DrawTreeChart
        public void DrawThisConcatAt(Point point)
        {
            //if(this.element.Length > 1 && this.element =="Concat")
            //NOTHING!!!DONE
            return;
        }


        //USING kCHAR * (int)emSizeChar, Pen>penEdgeWidth and g
        //Depending on subtrees too: split it
        public void DrawThisJoinAt(Point point)
        {
            Point startJoin = new Point(point.X, point.Y);// + root.rectSize.Height/2);
            startJoin.Offset(0, this.rectSize.Height / 2);
            Point endJoin = new Point(point.X, point.Y);
            endJoin.Offset(this.rectSize.Width, this.rectSize.Height / 2);

            ////int joinWidth = kCHAR * (int)emSizeChar + maxWidth + kCHAR * (int)emSizeChar;
            ////int joinHeight = arg1Tree.rectSize.Height + arg2Tree.rectSize.Height;

            ////tree.rectSize = new Size(joinWidth, joinHeight);

            //Define <<point0>> and <<point1>> for subtrees

            int d0 = (this.rectSize.Width - this.subTrees[0].rectSize.Width) / 2;//!!!

            Point point0 = new Point(point.X + d0, point.Y);//!!!function of <<point>> and <<this == root>>

            int d1 = (this.rectSize.Width - this.subTrees[1].rectSize.Width) / 2;//!!!

            Point point1 = new Point(point.X + d1, point.Y + this.subTrees[0].rectSize.Height);//!!!function of <<point>> and <<this == root>>

            //<<point0>> and <<point1>> defined

            //point,startJoin, endJoin,
            //point0, point1

            //1)Draw Line 0-. Before <<this.subTrees[0]|this.subTrees[1]>> !!!

            Point[] BJ = new Point[2];
            BJ[0] = new Point(startJoin.X, startJoin.Y);//+V for root
            BJ[1] = new Point(startJoin.X + kCHAR * (int)emSizeChar, startJoin.Y);//+Hollow Arrow?!

            Pen penEdge = new Pen(Color.Blue, penEdgeWidth);
            g.DrawLine(penEdge, BJ[0], BJ[1]);

            //2)Draw  startJoin== BJ[0]-Vertex!!!
#if DRAW_ADDITIONAL_VERTEX
            drawVertex(startJoin);
#endif
            //3)Draw Lines Before !!!
            Point[] B = new Point[4];
            B[0] = new Point(point0.X, point0.Y + this.subTrees[0].rectSize.Height / 2);//-V-Arrow
            B[1] = new Point(BJ[1].X, point0.Y + this.subTrees[0].rectSize.Height / 2);
            B[2] = new Point(BJ[1].X, point1.Y + this.subTrees[1].rectSize.Height / 2);
            B[3] = new Point(point1.X, point1.Y + this.subTrees[1].rectSize.Height / 2);//-V-Arrow

            g.DrawLines(penEdge, B);

            //4)Erase B[0]-Vertex and B[3]-Vertex as start points are topologically equivalent?????

            //2.1)Draw Line .->m After <<this.subTrees[0]|this.subTrees[1]>> !!!

            Point[] AJ = new Point[2];
            AJ[0] = new Point(endJoin.X - kCHAR * (int)emSizeChar, endJoin.Y);//
            AJ[1] = new Point(endJoin.X, endJoin.Y);// +arrow!!

            g.DrawLine(penEdge, AJ[0], AJ[1]);

            //2.2)Draw  endJoin== AJ[1]-Vertex!!!
#if DRAW_ADDITIONAL_VERTEX
            drawVertex(endJoin);
//#endif
            //2.3)Draw  Arrow at endJoin ==  AJ[1] !!!
            drawArrow(new Point(AJ[1].X - radius, AJ[1].Y));
#endif
            //2.4)Draw Lines AFTER  !!!
            Point[] A = new Point[4];
            A[0] = new Point(point0.X + this.subTrees[0].rectSize.Width, point0.Y + this.subTrees[0].rectSize.Height / 2);
            A[1] = new Point(endJoin.X - kCHAR * (int)emSizeChar, A[0].Y);
            A[2] = new Point(endJoin.X - kCHAR * (int)emSizeChar, point1.Y + this.subTrees[1].rectSize.Height / 2);
            A[3] = new Point(point1.X + this.subTrees[1].rectSize.Width, point1.Y + this.subTrees[1].rectSize.Height / 2);

            g.DrawLines(penEdge, A);

            //4)Erase A[0]-Vertex and A[3]-Vertex as start points are topologically equivalent?????
        }


        //USING fontSymbol>emSizeChar, element-simple
        public void drawSymbol_Element(Point pointLeftUpRect, string element)
        {
            Font fontSymbol = new Font(FontFamily.GenericMonospace, emSizeChar, FontStyle.Bold);

            this.g.DrawString(element, fontSymbol, Brushes.Red, pointLeftUpRect.X, pointLeftUpRect.Y);
        }

        //NOT USING
        public void drawVertex(Point p)
        {
            Rectangle rectStartVertex = new Rectangle(p.X - radiusVertex, p.Y - radiusVertex, 2 * radiusVertex, 2 * radiusVertex);
            g.FillEllipse(Brushes.Blue, rectStartVertex);//Brushes.Black
        }

        //USING depending on radiusVertex,Brush and g
        public void drawVertex(Point p, Brush brush)
        {
            Rectangle rectStartVertex = new Rectangle(p.X - radiusVertex, p.Y - radiusVertex, 2 * radiusVertex, 2 * radiusVertex);
            g.FillEllipse(brush, rectStartVertex);
        }

        //USING depending on emSizeChar,radiusVertex,n - nVertex, fontSymbol and g
        public void drawNumberVertex(Point p, int n)
        {
            int nW = 0, nH = 0;

            Font fontSymbol = new Font(FontFamily.GenericMonospace, emSizeChar, FontStyle.Bold);

            string str_n = string.Format("{0}", n);

            nW = (int)(str_n.Length * emSizeChar);
            nH = (int)(emSizeChar + 3);

            RectangleF rectNumberVertexF = new RectangleF((float)(p.X - radiusVertex), (float)(p.Y + radiusVertex),
                (float)nW, (float)nH);
            g.DrawString(str_n, fontSymbol, Brushes.Blue, rectNumberVertexF);

        }

        //USING depending on widthArrow,heightArrow,Brushes.Black and g
        public void drawArrow(Point p)
        {
            Point[] ArrowB1 = new Point[3];
            ArrowB1[0] = new Point(p.X, p.Y);
            ArrowB1[1] = new Point(ArrowB1[0].X - widthArrow, ArrowB1[0].Y - heightArrow);
            ArrowB1[2] = new Point(ArrowB1[0].X - widthArrow, ArrowB1[0].Y + heightArrow);

            g.FillPolygon(Brushes.Black, ArrowB1);
        }

        //USING depending on widthArrow,heightArrow,penEdgeWidth and g
        public void drawHollowArrow(Point p)
        {
            Pen pen = new Pen(Color.Blue);
            pen.Width = penEdgeWidth;

            Point[] ArrowB1 = new Point[3];
            ArrowB1[0] = new Point(p.X, p.Y);
            ArrowB1[1] = new Point(ArrowB1[0].X - widthArrow, ArrowB1[0].Y - heightArrow);
            ArrowB1[2] = new Point(ArrowB1[0].X - widthArrow, ArrowB1[0].Y + heightArrow);
            g.DrawPolygon(pen, ArrowB1);
        }

        //NOT USING
        public void drawInverseArrow(Point p)
        {
            Point[] ArrowB1 = new Point[3];
            ArrowB1[0] = new Point(p.X, p.Y);
            ArrowB1[1] = new Point(ArrowB1[0].X + 6, ArrowB1[0].Y - 3);
            ArrowB1[2] = new Point(ArrowB1[0].X + 6, ArrowB1[0].Y + 3);
            g.FillPolygon(Brushes.Black, ArrowB1);
        }

        //USING depending on widthArrow,heightArrow,Pen, penEdgeWidth and g
        public void drawHollowInverseArrow(Point p)
        {
            Pen pen = new Pen(Color.Blue);
            pen.Width = penEdgeWidth;

            Point[] ArrowB1 = new Point[3];
            ArrowB1[0] = new Point(p.X, p.Y);
            ArrowB1[1] = new Point(ArrowB1[0].X + widthArrow, ArrowB1[0].Y - heightArrow);
            ArrowB1[2] = new Point(ArrowB1[0].X + widthArrow, ArrowB1[0].Y + heightArrow);
            g.DrawPolygon(pen, ArrowB1);
        }

        //USING depending on widthArrow,heightArrow,Brushes.Black and g
        public void drawUpArrow(Point p)
        {
            Point[] ArrowB1 = new Point[3];
            ArrowB1[0] = new Point(p.X, p.Y);
            ArrowB1[1] = new Point(ArrowB1[0].X - heightArrow, ArrowB1[0].Y + widthArrow);
            ArrowB1[2] = new Point(ArrowB1[0].X + heightArrow, ArrowB1[0].Y + widthArrow);
            g.FillPolygon(Brushes.Black, ArrowB1);
        }

        //USING depending on widthArrow,heightArrow,Brushes.Black and g
        public void drawDownArrow(Point p)
        {
            Point[] ArrowB1 = new Point[3];
            ArrowB1[0] = new Point(p.X, p.Y);
            ArrowB1[1] = new Point(ArrowB1[0].X + heightArrow, ArrowB1[0].Y - widthArrow);
            ArrowB1[2] = new Point(ArrowB1[0].X - heightArrow, ArrowB1[0].Y - widthArrow);
            g.FillPolygon(Brushes.Black, ArrowB1);
        }



        //USING instead of DrawTreeChart

        //public void DrawTree (Point point,ref int  nVertex)
        //NO ENUMERATION startVertex and endVertex with GLOBAL nVertex
        //ATTRIBUTES of each node(this object) TreeChart:
        //a)Point point - Inherit
        //b)ref int nVertex - Inherit(local) and Synthesize(separate)(inflected form)or(to vary by inflection or transformation)
        //=> They are necessary for interpretation <TreeChart this> and <subtrees> when traversing them 
        //using the strategy <<top-down and left-right>>

        public void DrawTree(Point point, ref int nVertex)
        {
            //startVertex with with GLOBAL nVertex=0 and endVertex with GLOBAL nVertex++
            //this!=null whenever

            if (!(this.element == "Star" || this.element == "Concat" || this.element == "Join"))  // (this.subTrees==null))
            {
                this.DrawThisElementAt(point);
            }
            else switch (this.element)
                {
                    case "Star":

                        Point point0 = new Point(point.X + kCHAR * (int)emSizeChar,
                                                 point.Y + up_down_Star);//Correct
                        this.subTrees[0].g = this.g;

                        this.subTrees[0].DrawTree(point0, ref nVertex);//, this.subTrees[0] 

                        this.DrawThisStarAt(point, ref nVertex);//2)  //NO ENUMERATION TWO VERTEXES with GLOBAL <<nVertex>>

                        break;
                    case "Concat":

                        point0 = new Point(point.X,
                                           point.Y + (this.rectSize.Height - this.subTrees[0].rectSize.Height) / 2);

                        this.subTrees[0].g = this.g; //attribute down

                        //0)Draw  this.subTrees[0]

                        this.subTrees[0].DrawTree(point0, ref nVertex);//, this.subTrees[0]

                        Point point1 = new Point(point.X + this.subTrees[0].rectSize.Width,
                            point.Y + (this.rectSize.Height - this.subTrees[1].rectSize.Height) / 2);//function of <<point>> and <<this>>

                        Point pointConnect = new Point(point1.X, point1.Y + this.subTrees[1].rectSize.Height / 2);


                        //<<nVertex>> after TRAVERSING <<this.subTrees[0]>>
                        // It is the number of <<endVertex>> of <<this.subTrees[0]>>
                        //<<nVertex>> has be GLOBAL

                        drawNumberVertex(pointConnect, nVertex); //ENUMERATE one VERTEX with GLOBAL <<nVertex>>

                        //ENUMERATE
                        nVertex++;

                        this.subTrees[1].g = this.g; //attribute down

                        //1)Draw  this.subTrees[1]

                        this.subTrees[1].DrawTree(point1, ref nVertex);//, this.subTrees[0]                      

                        drawVertex(pointConnect, Brushes.Black);

                        //nVertex++;//??????

                        drawArrow(new Point(pointConnect.X - radiusVertex, pointConnect.Y));

                        break;
                    case "Join":

                        int D0 = (this.rectSize.Width - this.subTrees[0].rectSize.Width) / 2;
                        point0 = new Point(point.X + D0, point.Y);//function of <<point>> and <<this>>

                        this.subTrees[0].g = this.g; ///attribute down

                        this.subTrees[0].DrawTree(point0, ref nVertex);//, this.subTrees[0]

                        int D1 = (this.rectSize.Width - this.subTrees[1].rectSize.Width) / 2;
                        point1 = new Point(point.X + D1, point.Y + this.subTrees[0].rectSize.Height);

                        this.subTrees[1].g = this.g;///attribute down
                        this.subTrees[1].DrawTree(point1, ref nVertex);//, this.subTrees[1]

                        this.DrawThisJoinAt(point); //NO ENUMERATING, ENUMERATION with STAR and CONCAT
                        //Split it
                        break;
                    default:
                        break;
                }

        }//DrawTree


        //USING
        //Replaces GetTreeChart: any POLISH --> ready TREE, NO SIZE
        public static TreeChart PolishToTree(List<string> polish)
        {
            //USING
            //"PolishToTree:
            //ENUMERATION of VERTEXES is IMPOSSIBLE in VIEW of OPTIMIZATION 2,3
            //1)do <<reduction of strings for "Concat">> and 
            //2)do <<join> 1-strings-symbols for "Join"!!!");
            //3)do <<star>> <<star>> as <<star>>
            TreeChart treeChart = null;

            TreeChart arg1 = null;
            TreeChart arg2 = null;

            Stack<TreeChart> stack = new Stack<TreeChart>();
            List<string> listSimpleConcat = null;

            string joinS1S2 = "";

            foreach (string s in polish)
            {
                if (s.Length == 1)
                {

                    treeChart = new TreeChart(s, Size.Empty, null);
                    stack.Push(treeChart);
                }
                else if (s.Length == 0)
                {
                    //treeChart = new TreeChart("\"", Size.Empty, null);
                    treeChart = new TreeChart("", Size.Empty, null);
                    stack.Push(treeChart);
                }
                else switch (s)
                    {////s.Length > 1

                        case "Star": //(startPpoint,endPoint) in the middle
                            //TODO OPTIMIZATION arg=stack.Pop() if arg.element =="Star" then  arg STAR --> arg
                            arg1 = stack.Pop();
                            if (arg1.element == "Star")

                                treeChart = arg1;

                            else
                                treeChart = new TreeChart("Star", Size.Empty, new TreeChart[] { arg1 });//stack.Pop() 

                            stack.Push(treeChart);

                            break;
                        case "Concat"://(startPpoint,endPoint) in the middle
                            arg2 = stack.Pop();
                            arg1 = stack.Pop();

                            //NEVER OPTIMIZE                          

                            treeChart = new TreeChart("Concat", Size.Empty, new TreeChart[] { arg1, arg2 });//childs
                            stack.Push(treeChart);

                            break;
                        case "Join"://(startPpoint,endPoint) in the middle
                            arg2 = stack.Pop();
                            arg1 = stack.Pop();

                            //DONE: JoinSimpleOperand-OPTIMIZATION
                            //if( arg1 is <JoinSimpleOperand> && arg2 is <JoinSimpleOperand>) then arg1|arg2 is OPERAND, so
                            //arg1,arg2 --> arg1|arg2
                            if (!IsJoinSimpleOperand(arg1) || !IsJoinSimpleOperand(arg2))
                            {


                                treeChart = new TreeChart("Join", Size.Empty, new TreeChart[] { arg1, arg2 });//childs
                                stack.Push(treeChart);
                            }
                            else//(IsJoinSimpleOperand(arg1) && IsJoinSimpleOperand(arg2)) ; JoinSimpleOperand-OPTIMIZATION DONE
                            {//Boath Values <<rg1.element>> and <<arg2.element>> are strings in a form of
                                // "a" or "a0|a1|...|an" where ai is SYMBOL (ai.Length < 1)and ai != aj if i != j
                                joinS1S2 = JoinSimpleOperand(arg1.element, arg2.element);
                                treeChart = new TreeChart(joinS1S2, Size.Empty, null);//treeChart replaces  two trees arg1 and arg2
                                stack.Push(treeChart);
                            }


                            break;
                        default:
                            //s.Length > 1, s is an string-concatenation
                            //TODO: REDUCE

                            listSimpleConcat = StringToConcat(s);
                            foreach (string symbol in listSimpleConcat)
                            {
                                if (symbol.Length == 1)
                                {
                                    treeChart = new TreeChart(symbol, Size.Empty, null);
                                    stack.Push(treeChart);
                                }
                                else if (symbol.Length == 0)
                                {

                                    treeChart = new TreeChart("\"", Size.Empty, null);
                                    stack.Push(treeChart);
                                }
                                else //symbol == "Concat"
                                {
                                    arg2 = stack.Pop();
                                    arg1 = stack.Pop();
                                    treeChart = new TreeChart("Concat", Size.Empty, new TreeChart[] { arg1, arg2 });//childs
                                    stack.Push(treeChart);

                                }
                            }
                            break;
                    }
            }
            return treeChart;
        }

        //Using
        //ATTRIBUTES: necessary to study (See Troelsen)
        //a)<Size elementSize> - Sinthesize(only) + Local + Assignable
        //because it is necessary for next phase Drawing at the Point point
        //==> Each phase of Processing with some Attributes can be realized as a separate Thread!!!!
        //==> General Approach: 
        //any transformation is a special Proceccing with Attributes
        //There is a need in DEFINING initial kernel data structure that is invariant by all attribute transformation
        //or it is easy to be inflected!!!
        //Formal approach is based on a theory of multityped algebra and rewriting system!!!
        //XML-forms are more suitable as input and output for attribute trancformations, but
        //intermidiate working data structure for them as more suitable is a object document model:
        //Model-View-Controller or {Model-Attribute Transformation-Model}- View-Controller.
        //Such a model corresponds the classic scheme (graphic outline)of processing 
        //(Controller==({Model==(input(x[,y])-{process(x,y)})-View==(output([x,]y))}))
        //==>>>>>
        //Follow this sheme for the Algebra of Regular Expressions!!!
        //<<<<<==
        //Attribute Size Calculation
        public TreeChart AssignSizeToTree()
        {
            Size elementSize = Size.Empty;
            TreeChart arg1Tree = null;
            TreeChart arg2Tree = null;

            if (this == null)
                return null;

            //tree!=null

            if (!(this.element == "Star" || this.element == "Concat" || this.element == "Join"))
            {
                //tree.element.Length > 0 
                //res = new TreeChart(s, new Size(3 * widthChar, 2+widthChar+2), null);

                this.rectSize = new Size((kCHAR - 1) * (int)emSizeChar + this.element.Length * (int)emSizeChar + (kCHAR + 1) * (int)emSizeChar,
                                        2 * (4 + (int)emSizeChar + baseElement));//+ 4
                return this;
            };
            switch (this.element)
            {
                case "Star":
                    arg1Tree = this.subTrees[0].AssignSizeToTree();

                    //Size starSize 
                    this.rectSize = new Size(kCHAR * (int)emSizeChar + arg1Tree.rectSize.Width + kCHAR * (int)emSizeChar,
                            up_down_Star + arg1Tree.rectSize.Height + up_down_Star);

                    break;

                case "Concat":

                    arg1Tree = this.subTrees[0].AssignSizeToTree();
                    arg2Tree = this.subTrees[1].AssignSizeToTree();

                    //Size concatSize 
                    this.rectSize = new Size(arg1Tree.rectSize.Width + arg2Tree.rectSize.Width,
                        (arg1Tree.rectSize.Height > arg2Tree.rectSize.Height) ?
                        arg1Tree.rectSize.Height : arg2Tree.rectSize.Height);

                    break;

                case "Join":

                    arg1Tree = this.subTrees[0].AssignSizeToTree();
                    arg2Tree = this.subTrees[1].AssignSizeToTree();

                    //Size joinSize 
                    int maxWidth = (arg1Tree.rectSize.Width > arg2Tree.rectSize.Width) ?
                        arg1Tree.rectSize.Width : arg2Tree.rectSize.Width;

                    int joinWidth = kCHAR * (int)emSizeChar + maxWidth + kCHAR * (int)emSizeChar;
                    int joinHeight = arg1Tree.rectSize.Height + arg2Tree.rectSize.Height;

                    this.rectSize = new Size(joinWidth, joinHeight);

                    break;

                default:
                    break;
            }
            return this;
        }

        //Using
        static bool IsJoinSimpleOperand(TreeChart arg)
        {
            if (arg.element == "Star" || arg.element == "Concat" || arg.element == "Join")
                return false;
            return true;
        }

        //Using
        //TODO: NEED VERIFICATION
        public static string JoinSimpleOperand(string s1, string s2)
        {
            if (s1 == "" || s2 == "")//??????????????????????
                if (s1 == "")
                    return s2;
                else
                    return s1;

            string join = s1 + "|" + s2 + "|";
            int k = 0;
            for (int i = 0; i < join.Length; i += 2)
            {
                k = i + 2;
                while (k < join.Length)
                    if (join[i] == join[k])
                        join = join.Remove(k, 2);
                    else
                        k += 2;
            }
            return join.Remove(join.Length - 1);
        }

        //USING
        static List<string> StringToConcat(string expr)
        {
            List<string> res = new List<string>();
            if (expr.Length > 1)
            {

                res.Add(expr.Substring(0, 1));
                for (int i = 1; i < expr.Length; i++)
                {
                    res.Add(expr.Substring(i, 1));
                    res.Add("Concat");
                }
                return res;
            };
            return null;
        }

        //Print Polish with Size : TreeChart is AST actually!!!
        public void PrintTree()//TreeChart root
        {
            //this!=null whenever

            if (!(this.element == "Star" || this.element == "Concat" || this.element == "Join"))
            {
                // Console.WriteLine("<{0}> ({1:d2},{2:d2})", this.element, this.rectSize.Width, this.rectSize.Height);

                Console.Write("<{0}>\t", this.element);
                if (!this.rectSize.IsEmpty)
                    Console.WriteLine("({0:d3},{1:d3})", this.rectSize.Width, this.rectSize.Height);
                else
                    Console.WriteLine();
            }
            else switch (this.element)
                {
                    case "Star":

                        this.subTrees[0].PrintTree();
                        //Console.WriteLine("<{0}> ({1:d2},{2:d2})", this.element, this.rectSize.Width, this.rectSize.Height);
                        Console.Write("<{0}>\t", this.element);
                        if (!this.rectSize.IsEmpty)
                            Console.WriteLine("({0:d3},{1:d3})", this.rectSize.Width, this.rectSize.Height);
                        else
                            Console.WriteLine();

                        break;
                    case "Concat":
                        this.subTrees[0].PrintTree();
                        this.subTrees[1].PrintTree();

                        //Console.WriteLine("<{0}> ({1:d2},{2:d2})", this.element, this.rectSize.Width, this.rectSize.Height);
                        Console.Write("<{0}>\t", this.element);
                        if (!this.rectSize.IsEmpty)
                            Console.WriteLine("({0:d3},{1:d3})", this.rectSize.Width, this.rectSize.Height);
                        else
                            Console.WriteLine();

                        break;
                    case "Join":
                        this.subTrees[0].PrintTree();
                        this.subTrees[1].PrintTree();

                        //Console.WriteLine("<{0}> ({1:d2},{2:d2})", this.element, this.rectSize.Width, this.rectSize.Height);
                        Console.Write("<{0}>\t", this.element);
                        if (!this.rectSize.IsEmpty)
                            Console.WriteLine("({0:d3},{1:d3})", this.rectSize.Width, this.rectSize.Height);
                        else
                            Console.WriteLine();

                        break;
                    default:

                        break;
                }
            return;
        }

        public TreeChart GetExpression() //TreeChart this ~ this
        {
            // Evaluate the attribute TreeChart.RegExp as the infix form
            //There is possible to evaluate a new attribute in the postfix form or in the prefix form
            //this!=null whenever
            TreeChart res = null;

            if (!(this.element == "Star" || this.element == "Concat" || this.element == "Join"))
            {
                res = new TreeChart(this.element, Size.Empty, null);

                if (this.element.Length == 0)
                    //Console.Write("\"\"");
                    res.RegExp = "\"\"";
                else
                    //Console.Write("{0}", this.element);
                    if (this.element.Length > 1)
                        res.RegExp = this.element;
                    else//(this.element.Length ==1)
                        res.RegExp = this.element;

                return res;
            }
            else switch (this.element)
                {
                    case "Star"://DONE

                        TreeChart res0 = this.subTrees[0].GetExpression();

                        //Apply Equivalence Transformations

                        res = new TreeChart("Star", Size.Empty, null);
                        res.RegExp = "{" + res0.RegExp + "}";

                        break;
                    case "Concat": //???TODO: All to do is not to get redundant brackets

                        //1)GET Sources
                        res0 = this.subTrees[0].GetExpression();
                        TreeChart res1 = this.subTrees[1].GetExpression();


                        res = new TreeChart("Concat", Size.Empty, null);

                        bool bOpExp0 = (res0.element == "Star"
                                    || res0.element == "Concat"
                                    || res0.element == "Join");
                        ////!bOpExp0 - Simple

                        bool bOpExp1 = (res1.element == "Star"
                                    || res1.element == "Concat"
                                    || res1.element == "Join");
                        //!bOpExp1 - Simple

                        //Simple * Simple -> Simple+Simple

                        if (!bOpExp0 && !bOpExp1)
                        {

                            res.RegExp = res0.RegExp + res1.RegExp;
                            res.element = res.RegExp;
                            break;
                        };

                        //Simple * (Concat or Star) -> Simple+*+(Concat or Star)

                        if (!bOpExp0 && (res1.element == "Star" || res1.element == "Concat"))
                        {
                            res.RegExp = res0.RegExp + "*" + res1.RegExp;
                            break;
                        };

                        //(Concat or Star) * Simple   -> (Concat or Star)+*+Simple

                        if (!bOpExp1 && (res0.element == "Star" || res0.element == "Concat"))
                        {
                            res.RegExp = res0.RegExp + "*" + res1.RegExp;
                            break;
                        };

                        //(Concat or Star) * (Concat or Star)   -> (Concat or Star)+*+(Concat or Star)

                        if ((res0.element == "Star" || res0.element == "Concat")
                            && (res1.element == "Star" || res1.element == "Concat"))
                        {
                            res.RegExp = res0.RegExp + "*" + res1.RegExp;
                            break;
                        };

                        //Join   *  (Simple or Concat or Star) -> (+Join+) +*+(Simple or Concat or Star)

                        if (res0.element == "Join" && res1.element != "Join")
                        {
                            res.RegExp = "(" + res0.RegExp + ")" + "*" + res1.RegExp;
                            break;
                        };

                        //(Simple or Concat or Star) * Join -> (Simple or Concat or Star)+*+(+Join+)

                        if (res0.element != "Join" && res1.element == "Join")
                        {
                            res.RegExp = res0.RegExp + "*" + "(" + res1.RegExp + ")";
                            break;
                        };

                        //Join * Join ->(+Join+) +*+ (+Join+)
                        if (res0.element == "Join" && res1.element == "Join")
                        {
                            res.RegExp = "(" + res0.RegExp + ")" + "*" + "(" + res1.RegExp + ")";
                            break;
                        };

                        break;
                    case "Join":

                        //1)GET Sources

                        res0 = this.subTrees[0].GetExpression();
                        res1 = this.subTrees[1].GetExpression();

                        //Apply Equivalence Transformations
                        //(Simple or Join or Star or Concat) | (Simple or Join or Star or Concat)-> (All)+|+(All)

                        res = new TreeChart("Join", Size.Empty, null);
                        res.RegExp = res0.RegExp + "|" + res1.RegExp;

                        break;
                    default:

                        break;
                }
            return res;
        }

        public List<string> GetPostfixForm() //TreeChart this ~ this
        {
            // Evaluate the attribute TreeChart.RegExp as the infix form
            //There is possible to evaluate a new attribute in the postfix form or in the prefix form
            //this!=null whenever
            List<string> res = null;

            if (!(this.element == "Star" || this.element == "Concat" || this.element == "Join"))
            {
                res = new List<string>();

                if (this.element.Length == 0)
                    //Console.Write("\"\"");
                    res.Add("\"\"");
                else
                    //Console.Write("{0}", this.element);
                    if (this.element.Length > 1) //NEED TRANSFORMING "a0|...|an" into ("a0",...,"an")
                    {
                        string[] sjoined = this.element.Split('|');

                        res.Add(sjoined[0]);
                        for (int i = 1; i < sjoined.GetLength(0); i++)
                        {
                            res.Add(sjoined[i]);
                            res.Add("Join");
                        }
                    }
                    else//(this.element.Length ==1)
                        res.Add(this.element);

                return res;
            }
            else switch (this.element)
                {
                    case "Star"://DONE

                        //TreeChart res0 = this.subTrees[0].GetExpression();

                        List<string> res0 = this.subTrees[0].GetPostfixForm();

                        //Apply Equivalence Transformations

                        //res = new TreeChart("Star", Size.Empty, null);
                        //res.RegExp = "{" + res0.RegExp + "}";
                        //res = new List<string>();
                        res = res0;
                        res.Add(this.element);

                        break;
                    case "Concat": //???TODO
                        //1)GET Sources
                        //Direct
                        res0 = this.subTrees[0].GetPostfixForm();
                        List<string> res1 = this.subTrees[1].GetPostfixForm();
                        res = res0;
                        res.AddRange(res1);
                        res.Add(this.element);
                        break;

                    //res0 = this.subTrees[0].GetExpression();

                    //TreeChart res1 = this.subTrees[1].GetExpression();


                    //res = new TreeChart("Concat", Size.Empty, null);

                    //bool bOpExp0 = (res0.element == "Star"
                    //            || res0.element == "Concat"
                    //            || res0.element == "Join");
                    //////!bOpExp0 - Simple

                    //bool bOpExp1 = (res1.element == "Star"
                    //            || res1.element == "Concat"
                    //            || res1.element == "Join");
                    ////!bOpExp1 - Simple

                    ////Simple * Simple -> Simple+Simple

                    //if (!bOpExp0 && !bOpExp1)
                    //{

                    //    res.RegExp = res0.RegExp + res1.RegExp;
                    //    res.element = res.RegExp;
                    //    break;
                    //};

                    ////Simple * (Concat or Star) -> Simple+*+(Concat or Star)

                    //if (!bOpExp0 && (res1.element == "Star" || res1.element == "Concat"))
                    //{
                    //    res.RegExp = res0.RegExp + "*" + res1.RegExp;
                    //    break;
                    //};

                    ////(Concat or Star) * Simple   -> (Concat or Star)+*+Simple

                    //if (!bOpExp1 && (res0.element == "Star" || res0.element == "Concat"))
                    //{
                    //    res.RegExp = res0.RegExp + "*" + res1.RegExp;
                    //    break;
                    //};

                    ////(Concat or Star) * (Concat or Star)   -> (Concat or Star)+*+(Concat or Star)

                    //if ((res0.element == "Star" || res0.element == "Concat")
                    //    && (res1.element == "Star" || res1.element == "Concat"))
                    //{
                    //    res.RegExp = res0.RegExp + "*" + res1.RegExp;
                    //    break;
                    //};

                    ////Join   *  (Simple or Concat or Star) -> (+Join+) +*+(Simple or Concat or Star)

                    //if (res0.element == "Join" && res1.element != "Join")
                    //{
                    //    res.RegExp = "(" + res0.RegExp + ")" + "*" + res1.RegExp;
                    //    break;
                    //};

                    ////(Simple or Concat or Star) * Join -> (Simple or Concat or Star)+*+(+Join+)

                    //if (res0.element != "Join" && res1.element == "Join")
                    //{
                    //    res.RegExp = res0.RegExp + "*" + "(" + res1.RegExp + ")";
                    //    break;
                    //};

                    ////Join * Join ->(+Join+) +*+ (+Join+)
                    //if (res0.element == "Join" && res1.element == "Join")
                    //{
                    //    res.RegExp = "(" + res0.RegExp + ")" + "*" + "(" + res1.RegExp + ")";
                    //    break;
                    //};

                    //break;
                    case "Join":

                        //1)GET Sources
                        //Direct
                        res0 = this.subTrees[0].GetPostfixForm();
                        res1 = this.subTrees[1].GetPostfixForm();
                        res = res0;
                        res.AddRange(res1);
                        res.Add(this.element);
                        break;

                    //res0 = this.subTrees[0].GetExpression();
                    //res1 = this.subTrees[1].GetExpression();

                    ////Apply Equivalence Transformations
                    ////(Simple or Join or Star or Concat) | (Simple or Join or Star or Concat)-> (All)+|+(All)

                    //res = new TreeChart("Join", Size.Empty, null);
                    //res.RegExp = res0.RegExp + "|" + res1.RegExp;

                    //break;
                    default:

                        break;
                }
            return res;
        }

        //NO NEED, See TreeChart GetExpression() or GetPostfixForm() or wright new string GetInfixForm()
        static string strExpr = "";
        public static void PrintExpression(TreeChart root)
        {
            //root!=null whenever

            if (!(root.element == "Star" || root.element == "Concat" || root.element == "Join"))
            {
                if (root.element.Length == 0)
                {
                    Console.Write("\"\"");
                    strExpr = strExpr + "\"\"";
                }
                else
                {
                    Console.Write("{0}", root.element);
                    strExpr = strExpr + root.element;
                }
                //if (!root.rectSize.IsEmpty)
                //    Console.WriteLine("({0:d3},{1:d3})", root.rectSize.Width, root.rectSize.Height);
                //else
                //    Console.WriteLine();
            }
            else switch (root.element)
                {
                    case "Star":
                        Console.Write("{0}", "{");
                        strExpr = strExpr + "{";
                        PrintExpression(root.subTrees[0]);
                        Console.Write("{0}", "}");
                        strExpr = strExpr + "}";

                        //if (!root.rectSize.IsEmpty)
                        //    Console.WriteLine("({0:d3},{1:d3})", root.rectSize.Width, root.rectSize.Height);
                        //else
                        //    Console.WriteLine();

                        break;
                    case "Concat":
                        if (root.subTrees[0].element == "Join")
                        {
                            Console.Write("{0}", "(");
                            strExpr = strExpr + "(";
                            PrintExpression(root.subTrees[0]);
                            Console.Write("{0}", ")");
                            strExpr = strExpr + ")";
                        }
                        else
                            PrintExpression(root.subTrees[0]);

                        bool bOpExp0 = (root.subTrees[0].element == "Star" || root.subTrees[0].element == "Concat" || root.subTrees[0].element == "Join");

                        bool bOpExp1 = (root.subTrees[1].element == "Star" || root.subTrees[1].element == "Concat" || root.subTrees[1].element == "Join");

                        if (bOpExp0 || bOpExp1)
                        {
                            Console.Write("{0}", "*");
                            strExpr = strExpr + "*";
                        }
                        //else 
                        //Nothing to DO
                        //Replace root.element with root0.element+root1.element
                        //Change Type of root

                        if (root.subTrees[1].element == "Join")
                        {
                            Console.Write("{0}", "(");
                            strExpr = strExpr + "(";
                            PrintExpression(root.subTrees[1]);
                            Console.Write("{0}", ")");
                            strExpr = strExpr + ")";
                        }
                        else
                            PrintExpression(root.subTrees[1]);

                        //Console.Write("<{0}>\t", root.element);
                        //if (!root.rectSize.IsEmpty)
                        //    Console.WriteLine("({0:d3},{1:d3})", root.rectSize.Width, root.rectSize.Height);
                        //else
                        //    Console.WriteLine();

                        break;
                    case "Join":
                        bool bNoPair = true;
                        bNoPair = (root.subTrees[0].element != "Join" || root.subTrees[1].element != "Join");

                        if (!bNoPair)
                        {
                            Console.Write("{0}", "(");
                            strExpr = strExpr + "(";
                        }

                        PrintExpression(root.subTrees[0]);
                        Console.Write("{0}", "|");
                        strExpr = strExpr + "|";
                        PrintExpression(root.subTrees[1]);

                        if (!bNoPair)
                        {
                            Console.Write("{0}", ")");
                            strExpr = strExpr + ")";
                        }

                        //Console.Write("<{0}>\t", root.element);
                        //if (!root.rectSize.IsEmpty)
                        //    Console.WriteLine("({0:d3},{1:d3})", root.rectSize.Width, root.rectSize.Height);
                        //else
                        //    Console.WriteLine();

                        break;
                    default:

                        break;
                }
            return;
        }

        public static Bitmap DrawBitmap(ExprNameArray expression)
        {
            string[] arrPolish = expression.arrPolish;

            string RegExpName = expression.name;

            List<string> polishList = new List<string>(arrPolish);

            //=======Change "" for EMPTY STRING========
            for (int i = 0; i < polishList.Count; i++)
                if (polishList[i] != "\"\"")
                    continue;
                else
                    polishList[i] = "";
            //========================================        

#if NoTest
                Console.WriteLine("Source Polish Expression.");
                Console.WriteLine("PolishToTree: Do reduction for strings and Do join 1-strings-symbols!!!");

                foreach (string s in polish)
                    Console.WriteLine(s);
                Console.WriteLine();

                //Console.WriteLine("Press the Key Enter to Exit:");
                //Console.ReadKey();
                //return;

                //now USING
                Console.WriteLine("\n Now USING\nTree Chart attributed with size-RECTANGLES");

                //1) List<string> polish --(PolishToTree)--> TreeChart treeChart 
                Console.WriteLine("Getting Reduced Polish Expression with Simple Operands untill Joined");
                Console.WriteLine("and With no Size");
#endif

            TreeChart treeChart = TreeChart.PolishToTree(polishList);//No Size, No Numbers,ENUMERATION IMPOSSIBLE
            List<string> postfix = treeChart.GetPostfixForm();
#if NoTest
                Console.WriteLine("\nPostfix = treeChart.GetPostfixForm();");
                foreach (string sp in postfix)
                    Console.WriteLine(sp);
                Console.WriteLine();
                Console.ReadKey();

                //treeChart.PrintTree(); //With No Size No Numbers//Working
#endif

            //2) TreeChart treeChart --(AssignSizeToTree)--> TreeChart treeChart  

            treeChart = treeChart.AssignSizeToTree(); //Get Size, But No Changing Numbers, ENUMERATION IMPOSSIBLE
#if NoTest

                //TODO:treeChart.AssignNumbersToTree();
                Console.WriteLine("\nTree Chart With Size");

                treeChart.PrintTree(); //With Size ~ //PrintTree(treeChart); No Numbers;ENUMERATION POSSIBLE
#endif

            //2) TreeChart treeChart --> string treeChart.RegExp

            //Back to Expression from TreeChart treeChart.

            treeChart.RegExp = treeChart.GetExpression().RegExp;// ~ PrintExpression(treeChart);

            treeChart.namedPostfixExp = expression;

            //TreeChart.PrintExpression(treeChart); //Working
#if NoTest

                Console.WriteLine("\ntreeChart.element:{0}", treeChart.element);
                Console.WriteLine("treeChart.RegExp:={0}", treeChart.RegExp);
#endif



            treeChart.DiagramName = "Diagram of Regular Expression:" + RegExpName;
#if NoTest


                Console.WriteLine("TreeChart.DiagramName:={0}", treeChart.DiagramName);

#endif
            Point atPoint = new Point(20, 20);
            Bitmap bitmap = new Bitmap(600, 300);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            treeChart.g = g;
            int nVertex = 1;

            treeChart.DrawTree(atPoint, ref nVertex);

            //5.0)====Draw <<startVertex>> and  its  nVertex == 0     

            Point startVertex = new Point(atPoint.X, atPoint.Y + treeChart.rectSize.Height / 2);
            treeChart.drawVertex(startVertex, Brushes.Red);

            //------nVertex == 0

            treeChart.drawNumberVertex(startVertex, 0);

            //5.1)====Draw <<endVertex>> and  its  nVertex + 1 and input Arrow 
            Point endVertex = new Point(atPoint.X + treeChart.rectSize.Width, startVertex.Y);

            treeChart.drawVertex(endVertex, Brushes.Red);
            //------nVertex 
            //nVertex++;
            treeChart.drawNumberVertex(endVertex, nVertex);
            //------input Arrow 
            treeChart.drawArrow(new Point(endVertex.X - TreeChart.radiusVertex, endVertex.Y));
            // nVertex+1 -- number vertexes!!!

            //////////////////////////////////////////////////////////

            Font fontSymbol1 = new Font(FontFamily.GenericMonospace, 14.0F, FontStyle.Bold);
            SizeF sizeFDiaName = g.MeasureString(treeChart.DiagramName, fontSymbol1);

            //2)----- Draw <<DiagramName>>
            g.DrawString(treeChart.DiagramName, fontSymbol1, Brushes.Red,
                atPoint.X, treeChart.rectSize.Height + 50);

            //3)----- Draw <<RegExp>> for Diagram
            g.DrawString(treeChart.RegExp, fontSymbol1, Brushes.Black,
                atPoint.X, treeChart.rectSize.Height + 80);

            //4)----- Draw <<namedPostfixExp>> for Diagram

            string str_polish = TreeChart.PolishListToString(treeChart.namedPostfixExp.arrPolish);

            if (str_polish != "")
            {
                g.DrawString("Polish Expression:", fontSymbol1, Brushes.Black,
                    atPoint.X, treeChart.rectSize.Height + 120);
                g.DrawString(str_polish, fontSymbol1, Brushes.Black,
                    atPoint.X, treeChart.rectSize.Height + 140);
            }

            return bitmap;
        }

    }
}
