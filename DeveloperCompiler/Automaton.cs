using System;
using System.Collections.Generic;
//
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleFrontEnd
{
    public class VSet
    {
        public BitArray vSetArray;
        public VSet()
        {
            vSetArray = new BitArray(256);//32
        }
        public VSet(VSet vSet)
        {
            this.vSetArray = new BitArray(vSet.vSetArray);
        }
        public bool IsEqual(VSet vset)
        {            
            for (int i = 0; i < vset.vSetArray.Count; i++)
                if (this.vSetArray[i] != vset.vSetArray[i])
                    return false;
                else
                    continue;            
            return true;
        }
    }

    public class Automaton //from (diagrammPublic, classPartListPublic)
    {
/*
        int vEnd;//Given:
        SortedList<string, string> vavEdges;//Given:
*/
        //Given:
        public string nameExpression;
        private Diagramm diagrammPublic;

        //RegularExpList regExpList = Program.regularExpList;

        //Program.regularExpList.infAlphabetPartList.classPartList; //RESULT is Intersection of All Partitons!!! 
       
        //Given: 
        // for all: classPartListPublic.Count and Linear Odering!!!
        
        //private 
        public SortedList<char, SubsetClass> classPartListPublic;// = Program.regularExpList.infAlphabetPartList.classPartList;

        public Automaton(string nameExpression,Diagramm diagrammPublic, SortedList<char, SubsetClass> classPartListPublic)
        {
            this.nameExpression = nameExpression;
            this.diagrammPublic = diagrammPublic;
            this.classPartListPublic = classPartListPublic;
            Get_a_EdgesList();
        }
        /// <summary>
        /// TODO
        /// </summary>
        public Automaton(string fullPathAut)
        {
            this.fullPathAut = fullPathAut;
            bool bLoadAut = LoadAutPublicKeys(fullPathAut);
            //!bLoadAut==> What to do
        }
        public Automaton()
        {
            //this.fullPathAut = fullPathAut;
            //bool bLoadAut = LoadAutPublicKeys(fullPathAut);
            ////!bLoadAut==> What to do
        }
        public string fullPathAut;
        private SortedList<string, List<string>> a_EdgesList;//TODO

        List<VSet> dataBaseMoveStates;//TODO
        List<VSet> dataBaseStates;//TODO


        public List<List<int>> go;//TODO
        public List<List<string>> fgo;//TODO

        public List<int> Final;//TODO

        public List<int> Errors;//TODO
        public List<int> Active;//TODO

        private void  Get_a_EdgesList()
        {
             this.a_EdgesList = new SortedList<string, List<string>>(this.classPartListPublic.Count + 1);
            //Capacity: +1 for ""=lambda
            //1) Init
             a_EdgesList.Add("", new List<string>(10));
             string aStr;
             foreach (char a in classPartListPublic.Keys)
             {
                 //aStr = a.ToString();
                 aStr=SubsetClass.CharToString(a);
                 a_EdgesList.Add(aStr, new List<string>(10));
             }
             //2) Filling up <a_EdgesList>
             foreach (string edge in this.diagrammPublic.edgeList)
                 a_EdgesList[edge.Substring(10)].Add(edge);
            // {
            //     string aKey = edge.Substring(10);
            //     //int i_aKey = a_EdgesList.IndexOfKey(aKey);
            //     //a_EdgesList.Values[i_aKey].Add(edge);
            //     a_EdgesList[aKey].Add(edge);
            // }
            //return ;
        }

        public VSet move(VSet q, char a)
        {
            VSet qMove = new VSet();
            //string aString = a.ToString();

            ////List<string> a_List = a_EdgesList[a.ToString()];
            List<string> a_List = a_EdgesList[SubsetClass.CharToString(a)];

            foreach (string a_edge in a_List)
            {
                int v_start = int.Parse(a_edge.Substring(0, 5));
                if (q.vSetArray[v_start])
                    qMove.vSetArray.Set(int.Parse(a_edge.Substring(5, 5)), true);
            }

            return qMove;
        }

        public VSet close(VSet vSet)
        {
            VSet qClose = new VSet(vSet);//Init qClose
            
            List<string> lambda_List = a_EdgesList[""];
            bool b_Next=false;

            do {
                b_Next=false;
                foreach (string lambda_edge in lambda_List)
                {
                    int v_s = int.Parse(lambda_edge.Substring(0, 5));
                    int v_e = int.Parse(lambda_edge.Substring(5, 5));
                    if(!qClose.vSetArray[v_s])
                        continue;
                    //qClose.vSetArray[v_s]==true
                    if (qClose.vSetArray[v_e])
                        continue;
                    //(qClose.vSetArray[v_s]==true)&& (qClose.vSetArray[v_e]==false)
                        qClose.vSetArray.Set(v_e, true);
                    b_Next=true;
                }

            }while (b_Next);
            //qClose == [vSet]
            return qClose;
        }
        public void GetGo()
        {
            //(0)Step
            VSet q0 = new VSet();
            q0.vSetArray.Set(0, true);

            dataBaseMoveStates = new List<VSet>(10);
            dataBaseStates = new List<VSet>(10);

            dataBaseMoveStates.Add(q0);//{0} --> dbMove[0]
            q0 = close(q0);
            //dataBaseStates = new List<VSet>(10);
            dataBaseStates.Add(q0);//[{0}] --> dbStates[0]==[dbMove[0]]==q0

            int i_q = 0;
            go = new List<List<int>>(10);
            int m = classPartListPublic.Count;
            go.Add(new List<int>(m));

            //string ff;
            while (i_q < go.Count)
            {
                VSet qi = dataBaseStates[i_q];

                for (int k = 0; k < m; k++)
                {
                    
                    char ak = classPartListPublic.Keys[k];
                    //if (ak == '\xFF')
                    //    ff = SubsetClass.CharToString(ak);
                    VSet move_qi_ak = move(qi, ak);
                    VSet qi_ak = close(move_qi_ak);

                    //Compare:qi_ak and dataBaseStates[s]

                    //int j_qi_ak = dataBaseStates.IndexOf(qi_ak, 0, dataBaseStates.Count);
                    int j_qi_ak = this.FindIndexOfTransitionVSet(qi_ak, dataBaseStates);

                    if (j_qi_ak < 0)
                    {
                        //New state: qi_ak ==close(move(qi, ak))==close(move_qi_ak);
                        //So [move_qi_ak]==qi_ak
                        dataBaseMoveStates.Add(move_qi_ak);//move_qi_ak --> dbMove[j_qi_ak]

                        dataBaseStates.Add(qi_ak);//[move_qi_ak]== qi_ak--> dbStates[j_qi_ak]==[dbMove[j_qi_ak]]==qi_ak
                        go.Add(new List<int>(m));

                        j_qi_ak = dataBaseStates.Count - 1; //Index of New state <qi_ak> in <dataBaseStates>
                    }
                    go[i_q].Add(j_qi_ak);
                    //go[i_q][k] = j_qi_ak;
                }                
                i_q++;
            }
        }
        private int FindIndexOfTransitionVSet(VSet q, List<VSet> db)
        {            
            for (int i = 0; i < db.Count; i++)            
                if (q.IsEqual(db[i]))
                    return i;
                else
                  continue;          
            return -1;
        }

        public int GetFinals()
        {            
            this.Final = new List<int>(10);

            for (int i = 0; i < dataBaseStates.Count; i++)
                if (dataBaseStates[i].vSetArray[diagrammPublic.q_end])
                    Final.Add(i);

            return Final.Count;
        }
        public void GetActive()
        {
            this.Active = new List<int>(this.Final);

            int m=this.go[0].Count;
            int p;
            bool bNoActive=true;
            while (bNoActive)
            {
                bNoActive = false;
                for (int q = 0; q < this.go.Count; q++)
                    for (int a = 0; a < m; a++)
                    {
                        p = this.go[q][a];
                        if (this.Active.Contains(p) && !this.Active.Contains(q))
                        {
                            this.Active.Add(q);
                            bNoActive = true;
                            break;
                        }
                    }
            }
        }
        public void GetErrors()
        {
            this.Errors = new List<int>(10);
            for (int q = 0; q < this.go.Count; q++)
                if (!this.Active.Contains(q))
                    this.Errors.Add(q);
        }
        //
        //Not Printing List<VSet> dataBaseStates;
        //
        public void PrintFinals(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();
            streamWriter.Write("Finals = { ");
            foreach(int i in this.Final)
                streamWriter.Write("{0,4} ", i);
            streamWriter.WriteLine(" }");
        }

        public void PrintActiveErrors(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();
            streamWriter.Write("Active = { ");
            foreach (int i in this.Active )
                streamWriter.Write("{0,4} ", i);
            streamWriter.WriteLine(" }");

            streamWriter.WriteLine();
            streamWriter.Write("Erros = { ");
            foreach (int i in this.Errors )
                streamWriter.Write("{0,4} ", i);
            streamWriter.WriteLine(" }");
        }


        public void PrintGo(StreamWriter streamWriter) //New Extended Solution
        {
            streamWriter.Write("q:a |");
            string akStr;
            for (int k = 0; k < this.classPartListPublic.Count; k++)
            {
                akStr = SubsetClass.CharToString(this.classPartListPublic.Keys[k]);
                ////streamWriter.Write("{0,3}|", this.classPartListPublic.Keys[k]);
                streamWriter.Write("{0,4}|", akStr);
            }
            //Add [move]=close(move)
            streamWriter.Write("{0}", "[move(q,a)]=close(move(q,a))");
            //
             streamWriter.WriteLine(); 
 
            for (int i_q = 0; i_q < this.go.Count; i_q++)
            {
                streamWriter.Write("{0,4}|",i_q);
                for (int k = 0; k < this.go[i_q].Count; k++)
                {
                    streamWriter.Write("{0,4}|",this.go[i_q][k]);

                }
                //Add [move]=close(move)
                //[dataBaseMoveStates(i_q)] == dataBaseStates(i_q)
                PrintValueStates(streamWriter, i_q);
                //
                streamWriter.WriteLine();
            }
            //PrintFinals
        }
        private void PrintValueStates(StreamWriter streamWriter,int i_q)
        {
            //[dataBaseMoveStates(i_q)] == dataBaseStates(i_q) --> streamWriter
            //1) Get List<int> vertexList from VSet dataBaseMoveStates(i_q)
            List<int> vertexListMove = new List<int>(10);
            VSet vSet_i_q_Move = this.dataBaseMoveStates[i_q];
            for (int i = 0; i < vSet_i_q_Move.vSetArray.Count; i++)
                if (vSet_i_q_Move.vSetArray[i])
                    vertexListMove.Add(i);
            ////vSet_i_q_Move.vSetArray;
            //2) Get List<int> vertexList from VSet dataBaseStates(i_q)
            List<int> vertexListClose = new List<int>(10);
            VSet vSet_i_q_Close = this.dataBaseStates[i_q];
            for (int i = 0; i < vSet_i_q_Close.vSetArray.Count; i++)
                if (vSet_i_q_Close.vSetArray[i])
                    vertexListClose.Add(i);
            ////vSet_i_q_Close.vSetArray;
            //3)
            streamWriter.Write("[");
            for(int i=0; i< vertexListMove.Count-1; i++)
                streamWriter.Write("{0} ", vertexListMove[i]);
            if(vertexListMove.Count>0)
            streamWriter.Write("{0}", vertexListMove[vertexListMove.Count - 1]);
            streamWriter.Write("]");

            streamWriter.Write("==");
            //4)
            streamWriter.Write("{");
            for (int i = 0; i < vertexListClose.Count-1; i++)
                streamWriter.Write("{0} ", vertexListClose[i]);
            if (vertexListClose.Count > 0)
            streamWriter.Write("{0}", vertexListClose[vertexListClose.Count - 1]);
            streamWriter.Write("}");
        }

        public void PrintGoWithoutValueStates(StreamWriter streamWriter) //Old Simple Solution
        {
            streamWriter.Write("q:a |");
            string akStr;
            for (int k = 0; k < this.classPartListPublic.Count; k++)
            {
                akStr = SubsetClass.CharToString(this.classPartListPublic.Keys[k]);
                ////streamWriter.Write("{0,3}|", this.classPartListPublic.Keys[k]);
                streamWriter.Write("{0,4}|", akStr);
            }
            streamWriter.WriteLine();

            for (int i_q = 0; i_q < this.go.Count; i_q++)
            {
                streamWriter.Write("{0,4}|", i_q);
                for (int k = 0; k < this.go[i_q].Count; k++)
                {
                    streamWriter.Write("{0,4}|", this.go[i_q][k]);

                }
                streamWriter.WriteLine();
            }
            //PrintFinals
        }

        public void PrintFGo(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();
            streamWriter.Write("q:a |");
            string akStr;
            for (int k = 0; k < this.classPartListPublic.Count; k++)
            {
                akStr = SubsetClass.CharToString(this.classPartListPublic.Keys[k]);
                ////streamWriter.Write("{0,3}|", this.classPartListPublic.Keys[k]);
                streamWriter.Write("{0,4}|", akStr);
            }
            streamWriter.WriteLine();

            for (int i_q = 0; i_q < this.fgo.Count; i_q++)
            {
                streamWriter.Write("{0,4}|", i_q);
                for (int k = 0; k < this.fgo[i_q].Count; k++)
                {
                    streamWriter.Write("{0,4}|", this.fgo[i_q][k]);

                }
                streamWriter.WriteLine();
            }
            //PrintFinals
        }
        
        public void GetFGo()
        {
            int n = this.go.Count;
            int m = this.go[0].Count;
            fgo = new List<List<string>>(n);
            for (int q = 0; q < n; q++)
                fgo.Add(new List<string>(m));

            //ActiveTransition
            int p;
            for (int q = 0; q < n; q++)
                for (int k = 0; k < m; k++)
                {
                    p=go[q][k];
                    if (this.Active.Contains(q) && this.Active.Contains(p))
                    {
                        fgo[q].Add("Actv");
                        continue;
                    }
                    else if (this.Active.Contains(q) && !this.Final.Contains(q) && this.Errors.Contains(p))
                    {
                        fgo[q].Add("ErrL");
                        continue;
                    }
                    else if (this.Final.Contains(q) && this.Errors.Contains(p))
                    {
                        fgo[q].Add("EndL");
                        continue;

                    }
                    else
                    {
                        fgo[q].Add("NONE");
                        continue;
                    }

                }

        }

        public static Automaton GetAutInPublicFromAutInPrivate(Automaton M, Automaton m, SortedList<char, List<char>> m_a_aListPubChar)
        {
            /* from m1.fullPathAut as a result of LoadAutPublicKeys(fullPathAut) in ctor Automaton(fullPathAut)
            * GetAutInPublicFromAutInPrivate(Automaton M,Automaton m,SortedList<char, List<char>> m_a_aListPubChar)
            //m.fullPathAut; ready for M
             * 
            m.go; do in Public for M.go
             * 
            m.Final; do in Public for M
             * 
            m.Active; do in Public for M
             * 
            m.Errors; do in Public for M
             * 
            m.fgo; do in Public for M
             * 
            //m.classPartListPublic; ready for M
            */
            ////M.fullPathAut = m.fullPathAut; //ready
            ////M.classPartListPublic = P12.classPartList;//ready
            
            //a) m.go ==> M.go

            ////List<int> row = m.go[0];
            ////int go_q_a = row[2]; // a_2 ==> <a_2,List<char>> m_a_aListPubChar

            int publicAlphCount = 0;
            int privateAlphCount = m.classPartListPublic.Count;//==m_a_aListPubChar.Count

            for (int i = 0; i < privateAlphCount; i++)
            {
                int a_Value_i_Count = m_a_aListPubChar.Values[i].Count;
                publicAlphCount = publicAlphCount + a_Value_i_Count;
            }

            SortedList<char, List<int>> clmn_M_List = new SortedList<char, List<int>>(publicAlphCount);

            int statesCount = m.go.Count;


            foreach (char a in m_a_aListPubChar.Keys)
            {
                int i_a = m_a_aListPubChar.Keys.IndexOf(a);

                List<int> a_clmn_m = new List<int>(statesCount);

                for (int q = 0; q < statesCount; q++)
                    a_clmn_m.Add(m.go[q][i_a]);
                //a_clmn_m is ready
                //Clone a_clmn_m for each b in a_Public_List
                foreach (char b in m_a_aListPubChar.Values[i_a])
                    clmn_M_List.Add(b, a_clmn_m);
            }
            //clmn_M_List is ready
            //(clmn_M_List.Count == publicAlphCount);

            //clmn_M_List ==> M.go 

            M.go = new List<List<int>>(statesCount); //statesCount = m.go.Count
            for (int q = 0; q < statesCount; q++)
                M.go.Add(new List<int>(publicAlphCount));

            // M.go table is ready so fill in it!!!
            for (int q = 0; q < statesCount; q++)
                for (int i_a = 0; i_a < publicAlphCount; i_a++)
                {
                    int p = clmn_M_List.Values[i_a][q];
                    M.go[q].Add(p);
                    //M.go[q][i_a] == p
                }
            //clmn_M_List ==> M.go DONE
            ////M.go //ready

            M.Final = m.Final;
            M.Errors = m.Errors;
            M.Active = m.Active;
            //Get M.nameExpression from <M.fullPathAut> == <m.fullPathAut>
            M.nameExpression = m.nameExpression;

             //M.fgo; //do in Public for M
            M.GetFGo();
            
            return M;
        }
        //Not constructing an automaton as an intersection of m1 and m2
        //but creating Cartesian of m1 and m2 
        public static bool EqualsInPublicKeys(Automaton m1, Automaton m2)//not Ignoring classPartList
        {
            //?????
            //Partial Control of Alphabets 
            //No Control of Partitions as their not exist in files!
            //?????
            Automaton M1,M2;// null
            int m1Count = m1.classPartListPublic.Count;
            int m2Count = m2.classPartListPublic.Count;

            if (m1Count != m2Count)
                goto labelDoConverting; //1)Converting && Cartesian
                

            //m1Count == m2Count
            bool bEgualAlphabets =true;
            for (int i = 0; i < m1Count; i++)
                if (m1.classPartListPublic.Keys[i] != m2.classPartListPublic.Keys[i])
                {
                    bEgualAlphabets= false;
                    break;
                }

            if(!bEgualAlphabets)
                goto labelDoConverting; //2)Converting && Cartesian
            //
            //bEgualAlphabets=true
            //(m1.classPartListPublic.Keys == m1.classPartListPublic.Keys) but
            //(m1.classPartListPublic.Values == m1.classPartListPublic.Values) is UNKNOWN!!!

            int bitArrayCount = m1.classPartListPublic.Values[0].subset.Count; // as bEgualAlphabets=true
            bool bEqualPartitions = true;
            for (int i = 0; i < m1Count; i++) //m1Count == m2Count
            {
                for(int j=0; j< bitArrayCount; j++)
                    if (m1.classPartListPublic.Values[i].subset[j] != m2.classPartListPublic.Values[i].subset[j])
                    {
                        bEqualPartitions = false;
                        break;
                    }
                if(!bEqualPartitions)
                    break;
            }
            if (bEqualPartitions)
            {
                M1 = m1;
                M2 = m2;
                goto labelDoCartesian; //4)No Converting && Cartesian
            }
            //else 

            labelDoConverting: //(m1Count != m2Count) OR !bEgualAlphabets OR !bEqualPartitions

            //a) Do Public Partition
            AlphabetPartList P1 = new AlphabetPartList(m1.classPartListPublic);
            AlphabetPartList P2 = new AlphabetPartList(m2.classPartListPublic);

            AlphabetPartList P12 = RegularExpList.IntersectionTwoAlphabetPart(P1, P2);
            P1.GetPrivateAlphabetPresentation(P12);
            P2.GetPrivateAlphabetPresentation(P12);
           
            //b) Find Representation
            SortedList<char, List<char>> m1_a_aListPubChar = P1.classPartPresentationList;
            SortedList<char, List<char>> m2_a_aListPubChar = P2.classPartPresentationList;

            //c) Trancform m1 and m2 from Private Alphabets into M1 and M2 in Public Alphabet
            //Automaton 
                M1 = new Automaton();  //null fields         

            /* from m1.fullPathAut as a result of LoadAutPublicKeys(fullPathAut) in ctor Automaton(fullPathAut)
             * GetAutInPublicFromAutInPrivate(Automaton M,Automaton m,SortedList<char, List<char>> m_a_aListPubChar)
            m1.fullPathAut; ready for M1
            m1.go; do in Public for M1
            m1.Final; do in Public for M1
            m1.Active; do in Public for M1
            m1.Errors; do in Public for M1
            m1.fgo; do in Public for M1
            m1.classPartListPublic; ready for M1
             */
            M1.fullPathAut = m1.fullPathAut;
            M1.classPartListPublic = P12.classPartList;
            GetAutInPublicFromAutInPrivate(M1, m1, m1_a_aListPubChar);

           
            //Automaton 
                M2 = new Automaton(); //null fields 
            M2.fullPathAut = m2.fullPathAut;
            M2.classPartListPublic = P12.classPartList;
            GetAutInPublicFromAutInPrivate(M2, m2 , m2_a_aListPubChar);
            

            labelDoCartesian: //(m1Count == m2Count) && bEgualAlphabets && bEqualPartitions

            //d) Get Cartesian Product of (m1,m2) or (M1,M2)

            List<Automaton> mTwoList = new List<Automaton>(2);
            mTwoList.Add(M1);
            mTwoList.Add(M2);

            //only m1Count == m2Count && bEgualAlphabets==true && bEqualPartitions==true
            //others need converting Private Alphabets into Public Alphabets

            Cartesian cartesianTwo = new Cartesian(mTwoList, M1.classPartListPublic/* as Public for boath*/); 
            cartesianTwo.GetCgo();
            cartesianTwo.GetCFinals();
            if (cartesianTwo.bNoIntersection)
                return false;
            //Intersection exists!!
            // if (m1 == m2) ?
            int M1_Count = cartesianTwo.CFinal[0].Count;
            int M2_Count = cartesianTwo.CFinal[1].Count;
            if(M1_Count!=M2_Count)
                return false;
            //(m1_Count==m2_Count)
            
            List<int> cartF1 = cartesianTwo.CFinal[0];
            List<int> cartF2 = cartesianTwo.CFinal[1];
            cartF1.Sort();
            cartF2.Sort();

            for(int i=0; i<M1_Count; i++)
                if(cartF1[i]!=cartF2[i])
                    return false;
            return true;
        }
        
        //for LOADIND Automation from file
        struct StructBitClassPart
        {
            public int index;
            public char aClass;
            public BitArray bitClass;
            public int classCount;
        } ;

        static StructBitClassPart getBitClassPartition(string strClass)
        {
            StructBitClassPart classBit = new StructBitClassPart();
            classBit.bitClass = new BitArray(256);

            int iColon = strClass.IndexOf(':');
            //1)
            int classIndex = int.Parse(strClass.Substring(6, iColon - 5 + 1).TrimEnd(')', ' ', ':'));
            //RESULT (1)
            classBit.index = classIndex;

            //2)
            string defSubset = strClass.Substring(iColon + 1);
            int iEqual = defSubset.IndexOf("= [");
            string sSymbol = defSubset.Substring(1, iEqual - 2);

            char aSymbol = GeneratorCIL.StringHex12ToChar(sSymbol);
            //RESULT (2)
            classBit.aClass = aSymbol;

            //3)
            string subsetAlph = defSubset.Substring(iEqual + 2);//3)

            //4)
            //subsetAlph ==> subsetAlphNoHex
            //string \xhh ==> char c
            string tail = subsetAlph;
            while (true)
            {
                int iSlashX = tail.IndexOf(@"\x");
                if (iSlashX < 0)
                    break;
                string hexC = tail.Substring(iSlashX, 4);
                char cHex = GeneratorCIL.StringHex12ToChar(hexC);
                tail = (tail.Substring(0, iSlashX) + cHex) + tail.Substring(iSlashX + 4);
            }

            string subsetAlphNoHex = tail;

            //5)
            //Read CiCi+1 if Ci+1='-' then Ci-Ci+2; i = i+3 else Ci; i=i+1

            int i = 1;

            while (true)
            {
                if (i >= subsetAlphNoHex.Length - 1)
                    break;
                char Ci = subsetAlphNoHex[i];
                char Ci_1 = subsetAlphNoHex[i + 1];

                if (Ci_1 == '-')
                {
                    char Ci_2 = subsetAlphNoHex[i + 2];
                    //RESULT (3)
                    for (int k = (int)Ci; k <= (int)Ci_2; k++)
                        classBit.bitClass[k] = true;
                    i = i + 3;
                }
                else
                {
                    //RESULT (3)
                    classBit.bitClass[(int)Ci] = true;
                    i = i + 1;
                }
            }
            //-6
            //RESULT (4)
            int classCount = 0;
            for (int k = 0; k < classBit.bitClass.Count; k++)
                classCount = classCount + ((classBit.bitClass[k]) ? 1 : 0);

            classBit.classCount = classCount;
            return classBit;
        }

        public bool LoadAutPublicKeys(string fullPathAut)//Modified for Value States
        {
            ////To Console
            if (File.Exists(fullPathAut))
            {
                Console.WriteLine("fullPathAut: <{0}>", fullPathAut);                
            }
            else
            {
                Console.WriteLine("fullPathAut: <{0}>", "Not Exisits");
                return false;
            }
            ////
           
            StreamReader streamReader = File.OpenText(fullPathAut);
            //0)
            this.fullPathAut = fullPathAut;
            //
/*
Automaton in Private Alphabet of Named Expression
Id = 
q:a |   9|   a|\xff|[move(q,a)]=close(move(q,a))
   0|   1|   2|   1|[0]=={0}
   1|   1|   1|   1|[]=={}
   2|   3|   3|   1|[1]=={1 2 3}
   3|   3|   3|   1|[2]=={2 3}

*/
            string line1;
            string line2NameExpAut;
            string line3PublicAlphabet_go;
            List<string> goLines = new List<string>(10);
            string lineFinals; //Last line of Input file <fullPathAut>

            line1 = streamReader.ReadLine();

            line2NameExpAut = streamReader.ReadLine();

            this.nameExpression = line2NameExpAut.TrimEnd(' ', '=');//TODO

            line3PublicAlphabet_go = streamReader.ReadLine();
            //q:a |   9|   a|\xff|[move(q,a)]=close(move(q,a))

            int lenRowGo = 0;
            //int indMove = line3PublicAlphabet_go.IndexOf("|[move(");
            int indMove = line3PublicAlphabet_go.LastIndexOf('|');
                //IndexOf("|[move(");
            string line3PublicAlphabet_go_NoMove = line3PublicAlphabet_go.Substring(0, indMove + 1);
            //lenRowGo = line3PublicAlphabet_go.Length;
            lenRowGo = line3PublicAlphabet_go_NoMove.Length;

            string lineGo;
            while (!streamReader.EndOfStream)
            {
                lineGo = streamReader.ReadLine();
                if (lineGo == "")
                    break;
                goLines.Add(lineGo);
            }

            lineFinals = streamReader.ReadLine();//Last line of Input
            //STOP READING INPUT FILE <fullPathAut> at the line before the line <Active >

            streamReader.ReadLine();
            string lineActive = streamReader.ReadLine();

            streamReader.ReadLine();
            string lineErros = streamReader.ReadLine();

            streamReader.ReadLine();
            //Input <fgoLines>
            string line3PublicAlphabet_fgo = streamReader.ReadLine();
            List<string> fgoLines = new List<string>(10);
            while (!streamReader.EndOfStream)
            {
                lineGo = streamReader.ReadLine();
                if (lineGo == "")
                    break;
                fgoLines.Add(lineGo);
            }            

            ////To Console
                Console.WriteLine(line1);
                Console.WriteLine(line2NameExpAut);
                Console.WriteLine(line3PublicAlphabet_go);
                foreach (string sgo in goLines)
                    Console.WriteLine(sgo);
                Console.WriteLine(lineFinals);

                //Test the line <Active >
                Console.WriteLine(lineActive);
                Console.WriteLine(lineErros);

                //Test the lines <fgo >
                Console.WriteLine(line3PublicAlphabet_fgo);
                foreach (string sfgo in fgoLines)
                    Console.WriteLine(sfgo);

                //Test the line <Partition>

                string linePartition = streamReader.ReadLine();
                streamReader.ReadLine();//Skip ""
                Console.WriteLine(linePartition);

                //Test and Get the lines <classPartList>  <Keys> and <Values>
                int alphabetCount = lenRowGo / 5 - 1;
                //1)==> //7)Get <classPartList> 

                //SortedList<char, SubsetClass> 
                    this.classPartListPublic = new SortedList<char, SubsetClass>(alphabetCount);
                //getClassPartListKeysPublic(line3PublicAlphabet_go);           

                for (int i = 0; i < alphabetCount; i++)
                {
                    //First Line
                    string lineClass = streamReader.ReadLine();      //Class(0) : 9 = [0-9]
                    Console.WriteLine(lineClass);
                    ////                  
                   Automaton.StructBitClassPart structBitClassPart = Automaton.getBitClassPartition(lineClass);

                   //structBitClassPart.aClass;  //Used for char Keys[i]
                   //structBitClassPart.bitClass;//Used for SubsetClass Value[i]

                   SubsetClass subsetClass = new SubsetClass(structBitClassPart.bitClass);
                   ////subsetClass.subset = structBitClassPart.bitClass;//Used for SubsetClass


                   this.classPartListPublic.Add(structBitClassPart.aClass, //char Keys[i]
                                                subsetClass);              //SubsetClass Value[i]

                   SubsetClass subsetClass_i = this.classPartListPublic.Values[i];
                    ////                 
                   
                   Console.WriteLine("Class({0}) : {1} = {2}",i,SubsetClass.CharToString(structBitClassPart.aClass),subsetClass.subsetToString()) ;

                   //Second Line
                    string lineClassCount = streamReader.ReadLine(); //subset.Count :(10)                   
                    Console.WriteLine(lineClassCount);
                    Console.WriteLine("structBitClassPart.classCount: ({0})", structBitClassPart.classCount);

                    //Skip Third Line
                    streamReader.ReadLine(); 
                }


                ///////////////////////////
                Console.WriteLine("length:{0} count of columns:{1}", lenRowGo, lenRowGo / 5);
                streamReader.Close();
            ////

            //1) Get <classPartList> only <Keys> not <Values> //OLD FIRST VARIANT

            //////SortedList<char, SubsetClass> classPartListPublic = getClassPartListKeysPublic(line3PublicAlphabet_go);           
            //////this.classPartListPublic = classPartListPublic;

            //2)Get <go>

            List<List<int>> go = getGo(goLines);            
            this.go = go;

            //3)Get <Final>

            List<int> Final = getFinalOrActiveOrErrors(lineFinals);            
            this.Final = Final;

            //====Additional Properties of Automation

            //4)Get <Active>
            List<int> Active = getFinalOrActiveOrErrors(lineActive);
            this.Active = Active;

            //5)Get <Errors>
            List<int> Errors = getFinalOrActiveOrErrors(lineErros);
            this.Errors = Errors;

            //6)Get <fgo>
            List<List<string>> fgo = getFgo(fgoLines);
            this.fgo = fgo;

            //7)Get <classPartListPublic.Values> related to <classPartListPublic.Keys>
            //classPartListPublic.Keys[i] is ready and classPartListPublic.Valus[i]==null *!!!*

            return true;
        }

        public  SortedList<char, SubsetClass> getClassPartListKeysPublic(string line3PublicAlphabet)
        {
            SortedList<char, SubsetClass> classPartListPublic =
                new SortedList<char, SubsetClass>(line3PublicAlphabet.Length / 5 - 1);

            for (int i = 1; i < line3PublicAlphabet.Length / 5; i++)
            {
                string si = line3PublicAlphabet.Substring(5 * i, 4);
                char ci = GeneratorCIL.StringHex12ToChar(si.TrimStart(' '));
                classPartListPublic.Add(ci, null);
            }
            return classPartListPublic;
        }

        public  List<List<int>> getGo(List<string> goLines)//Modified for Value States
        {
            int rowCount = goLines.Count;
//   0|   1|   2|   1|[0]=={0}
            int lenRowGo = 0;
            //int indMove = goLines[0].IndexOf("|[0]");
            int indMove = goLines[0].LastIndexOf('|');
                //IndexOf("|[0]");
            string goLines_0_NoMove = goLines[0].Substring(0, indMove + 1);            
            lenRowGo = goLines_0_NoMove.Length;
            int clmnCount = lenRowGo / 5 - 1;
            //int clmnCount = goLines[0].Length / 5 - 1;

            List<List<int>> go = new List<List<int>>(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                go.Add(new List<int>(clmnCount));
                for (int k = 0; k < clmnCount; k++)
                {
                    string sk = goLines[i].Substring(5 * (k + 1), 4);
                    go[i].Add(int.Parse(sk));
                }
            }
            return go;
        }
        public List<List<string>> getFgo(List<string> fgoLines)
        {
            int rowCount = fgoLines.Count;
            int clmnCount = fgoLines[0].Length / 5 - 1;

            List<List<string>> fgo = new List<List<string>>(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                fgo.Add(new List<string>(clmnCount));
                for (int k = 0; k < clmnCount; k++)
                {
                    string sk = fgoLines[i].Substring(5 * (k + 1), 4);
                    fgo[i].Add(sk);
                }
            }
            return fgo;
        }

        public  List<int> getFinalOrActiveOrErrors(string lineSubsetOfStates)
        {
            List<int> Final = new List<int>(10);
            int iOpen = lineSubsetOfStates.IndexOf('{');
            string sfinal = lineSubsetOfStates.Substring(iOpen).TrimStart('{').TrimEnd('}', ' ');
            int subsetCount = sfinal.Length / 5;

            for (int i = 0; i < subsetCount; i++)
                Final.Add(int.Parse(sfinal.Substring(i * 5, 5)));
            return Final;
        }


    }

    public class Cartesian
    {
        //Given:
        private List<Automaton> automatonList; //in Public Alphabet
        private SortedList<char, SubsetClass> classPartListPublic;// = Program.regularExpList.infAlphabetPartList.classPartList;

        public Cartesian(List<Automaton> automatonList, SortedList<char, SubsetClass> classPartListPublic)
        {
            this.automatonList = automatonList;
            this.classPartListPublic = classPartListPublic;
            //Get_a_EdgesList();//NO NEED
        }
        //TODO
        //private SortedList<string, List<string>> a_EdgesList;//NO NEED

        public int CardinalityFinals;
        public bool bNoIntersection;

        List<List<int>> dataBaseStates;//TODO DONE

        public List<List<int>> Cgo;//TODO DONE
        public List<List<string>> Cfgo;//TODO DONE

        public List<List<int>> CFinal;//TODO DONE

        public List<int> CErrors;//TODO DONE
        public List<int> CActive;//TODO DONE
/*
        private void Get_a_EdgesList()//NO NEED
        {
            this.a_EdgesList = new SortedList<string, List<string>>(this.classPartListPublic.Count + 1);
            //Capacity: +1 for ""=lambda
            //1) Init
            a_EdgesList.Add("", new List<string>(10));  //Redundant
            string aStr;
            foreach (char a in classPartListPublic.Keys)
            {
                //aStr = a.ToString();
                aStr = SubsetClass.CharToString(a);
                a_EdgesList.Add(aStr, new List<string>(10));
            }
            //2) Filling up <a_EdgesList>
            foreach (string edge in this.diagrammPublic.edgeList)
                a_EdgesList[edge.Substring(10)].Add(edge);
            // {
            //     string aKey = edge.Substring(10);
            //     //int i_aKey = a_EdgesList.IndexOfKey(aKey);
            //     //a_EdgesList.Values[i_aKey].Add(edge);
            //     a_EdgesList[aKey].Add(edge);
            // }
            //return ;
        }
*/
        private int FindIndexOfTransitionVector(List<int> q, List<List<int>> db)//TODO DONE
        {
            for (int i = 0; i < db.Count; i++)
            {
                List<int> db_i = db[i];
                //q==db_i is true?
                bool bEqul = true;
                for (int j = 0; j < q.Count; j++)
                    if (q[j] == db_i[j])
                        continue;
                    else
                    {
                        bEqul = false;
                        break;
                    }
                if (bEqul)
                    return i;
            }

            return -1;
        }
  
        public void GetCgo() //TODO DONE
        { //(0)Step
            //r > 1
            int r = this.automatonList.Count;
            List<int> q0 = new List<int>(r);
            for (int i = 0; i < r; i++)
                            q0.Add(0);

            dataBaseStates = new List<List<int>>(20);
            dataBaseStates.Add(q0);
            int i_q = 0;
            this.Cgo = new List<List<int>>(20);
            int m = classPartListPublic.Count;//m is Public Alphabet Cardinality 
            this.Cgo.Add(new List<int>(m)); // 0-Row
            while (i_q < Cgo.Count)
            {
                List<int> qi = dataBaseStates[i_q];
                for (int k = 0; k < m; k++)
                {
                    List<int> qi_ak = new List<int>(r);
                    for (int j = 0; j < r; j++)
                        qi_ak.Add(this.automatonList[j].go[qi[j]][k]);
                    //(qi,ak,qi_ak)-new edge
                    // Is it qi_ak in dataBaseStates?
                    int s_qi_ak = this.FindIndexOfTransitionVector(qi_ak, dataBaseStates);
                    if (s_qi_ak < 0)
                    {
                        dataBaseStates.Add(qi_ak);
                        Cgo.Add(new List<int>(m));

                        s_qi_ak = dataBaseStates.Count - 1;//Last index of both Cgo and dataBaseStates
                    }
                    Cgo[i_q].Add(s_qi_ak);
                    //so Cgo[i_q][k] == s_qi_ak;
                }
                i_q++;//Next state!
            }
        }
        
        public bool GetCFinals()//TODO DONE
        {
            int r=this.automatonList.Count ;
            //Init
            this.CFinal = new List<List<int>>(r);
            
            for (int i = 0; i < r; i++)
                CFinal.Add(new List<int>(10));
            //
            int n = 0;
            BitArray states = new BitArray(dataBaseStates.Count);
            bool bIntersection=false;
            for (int q = 0; q < dataBaseStates.Count; q++)
            {

                List<int> vector_q = dataBaseStates[q];
                //
                for (int i = 0; i < r; i++)
                {
                    //if vector_q[i] in Fi then q in CFinal[q]
                    List<int> autFi = this.automatonList[i].Final;
                    if (autFi.Contains(vector_q[i]))
                    {
                        CFinal[i].Add(q);
                        if (states[q])
                            bIntersection = true; //as states[q] && states.Set(q, true);
                        else
                        {
                            states.Set(q, true);// as q is final!!!
                            n++;//for first final
                        }
                    }
                }

            }
            //Test Intersection                        
            this.CardinalityFinals = n;          
           
            this.bNoIntersection = !bIntersection;
            return !bIntersection;           
        }

        public void GetCActive()//TODO DONE
        {
             //(0) Init this.CActive with this.CFinal
            this.CActive = new List<int>(this.Cgo.Count - 1);

            //this.CActive <-- this.CFinal
            for(int j=0; j<this.CFinal.Count ; j++)
                for(int i=0; i<CFinal[j].Count; i++)
                    CActive.Add(CFinal[j][i]);
            //
            int m = this.Cgo[0].Count;
            int p;
            bool bNoActive = true;
            while (bNoActive)
            {
                bNoActive = false;
                for (int q = 0; q < this.Cgo.Count; q++)
                    for (int a = 0; a < m; a++)
                    {
                        p = this.Cgo[q][a];
                        if (this.CActive.Contains(p) && !this.CActive.Contains(q))
                        {
                            this.CActive.Add(q);
                            bNoActive = true;
                            break;
                        }
                    }
            }
        }

        public void GetCErrors()//TODO DONE
        {
            this.CErrors = new List<int>(10);
            for (int q = 0; q < this.Cgo.Count; q++)
                if (!this.CActive.Contains(q))
                    this.CErrors.Add(q);
        }

        private int CFinalContains(int q)//TODO DONE
        {
            int r = this.automatonList.Count;
            for (int i = 0; i < r; i++)
                for (int j = 0; j < this.CFinal[i].Count; j++)
                    if (this.CFinal[i].Contains(q))
                        return i; //first i: (q in CFinal[i])

            return -1;
        }
        public void GetFCgo()//TODO DONE
        {
            int n = this.Cgo.Count;
            int m = this.Cgo[0].Count;
            Cfgo = new List<List<string>>(n);
            for (int q = 0; q < n; q++)
                Cfgo.Add(new List<string>(m));

            //Transition
            int p;
            for (int q = 0; q < n; q++)
                for (int k = 0; k < m; k++)
                {
                    p = Cgo[q][k];
                    if (this.CActive.Contains(q) && this.CActive.Contains(p))
                    {
                        Cfgo[q].Add("Actv");
                        continue;
                    }
                    else 
                    {
                        int iF_q = this.CFinalContains(q);
                        if ((iF_q >=0) && this.CErrors.Contains(p))
                        {

                            Cfgo[q].Add(string.Format("L{0,3:d3}",iF_q));
                            continue;
                        }
                        else if((iF_q < 0) && this.CActive.Contains(q) && this.CErrors.Contains(p))
                        {
                             Cfgo[q].Add("ErrL");
                            continue;

                        }
                        else
                        {
                             Cfgo[q].Add("NONE");
                        continue;
                        }
                    }

                }

        }

        //
        //Not Printing List<List<int>> dataBaseStates;
        //
        // Find only existing (M_i,M_j,q), i<j
        public void FindPrintIntersectionsCFinals(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();

            int r = this.automatonList.Count;

            //(r==this.CFinal.Count)==>

            for (int i = 0; i < this.CFinal.Count; i++)
            {
                string nameM_i = automatonList[i].nameExpression;

                for (int j = i + 1; j < this.CFinal.Count; j++)
                {
                    string nameM_j = automatonList[j].nameExpression;
                    //Get  this.CFinal[i];
                    //Get  this.CFinal[j];
                    foreach(int q in this.CFinal[j])
                        if (this.CFinal[i].Contains(q))
                        {
                            streamWriter.WriteLine("Automata <{0}> and <{1}> is not separated: q={2}", nameM_i, nameM_j,q);
                            break;
                        }

                }
                ////streamWriter.WriteLine("{0}:", nameM_i);
                ////streamWriter.Write("Finals[{0}] = {1} ", i, '{');

                ////for (int j = 0; j < this.CFinal[i].Count; j++)
                ////{
                ////    int q = this.CFinal[i][j];
                ////    streamWriter.Write("{0,4} ", q);

                ////}

                ////streamWriter.WriteLine(" }");
                ////streamWriter.WriteLine();
            }

        }

        public void PrintCFinals(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();

            int r = this.automatonList.Count;

            //(r==this.CFinal.Count)==>

            for (int i = 0; i < this.CFinal.Count; i++)
            {
                string nameM_i = automatonList[i].nameExpression;
                streamWriter.WriteLine("{0}:", nameM_i);
                streamWriter.Write("Finals[{0}] = {1} ", i, '{');

                for (int j = 0; j < this.CFinal[i].Count; j++)
                {
                    int q = this.CFinal[i][j];
                    streamWriter.Write("{0,4} ", q);

                }

                streamWriter.WriteLine(" }");
                streamWriter.WriteLine();
            }
        }

        //PrintCActiveCErrors == PrintActiveErrors (CActive,CErrors)
        public void PrintCActiveCErrors(StreamWriter streamWriter)
        {
            //streamWriter.WriteLine();

            streamWriter.Write("Active = { ");
            foreach (int i in this.CActive)
                streamWriter.Write("{0,4} ", i);
            streamWriter.WriteLine(" }");

            streamWriter.WriteLine();

            streamWriter.Write("Erros = { ");
            foreach (int i in this.CErrors)
                streamWriter.Write("{0,4} ", i);
            streamWriter.WriteLine(" }");
        }

        //PrintCgo == PrintGo(Cgo)
        public void PrintCgo(StreamWriter streamWriter) //New Solution with Value States
        {
            streamWriter.Write("q:a |");
            string akStr;
            for (int k = 0; k < this.classPartListPublic.Count; k++)
            {
                akStr = SubsetClass.CharToString(this.classPartListPublic.Keys[k]);
                ////streamWriter.Write("{0,3}|", this.classPartListPublic.Keys[k]);
                streamWriter.Write("{0,4}|", akStr);
            }
            //Add Vector of States
            streamWriter.Write("Vector");
            streamWriter.WriteLine();

            for (int i_q = 0; i_q < this.Cgo.Count; i_q++)
            {
                streamWriter.Write("{0,4}|", i_q);
                for (int k = 0; k < this.Cgo[i_q].Count; k++)
                {
                    streamWriter.Write("{0,4}|", this.Cgo[i_q][k]);

                }
                //Add Vector of States from List<List<int>> dataBaseStates;
                streamWriter.Write("<");
                List<int> vectorList = this.dataBaseStates[i_q];
                for(int i=0; i< vectorList.Count-1; i++)
                    streamWriter.Write("{0,4} ", vectorList[i]);
                streamWriter.Write("{0,4}", vectorList[vectorList.Count - 1]);
                streamWriter.Write(">");
                streamWriter.WriteLine();
            }
            //PrintCFinals
        }
        public void PrintCgoWithoutValueStates(StreamWriter streamWriter)//Old Simple Solution
        {
            streamWriter.Write("q:a |");
            string akStr;
            for (int k = 0; k < this.classPartListPublic.Count; k++)
            {
                akStr = SubsetClass.CharToString(this.classPartListPublic.Keys[k]);
                ////streamWriter.Write("{0,3}|", this.classPartListPublic.Keys[k]);
                streamWriter.Write("{0,4}|", akStr);
            }
            streamWriter.WriteLine();

            for (int i_q = 0; i_q < this.Cgo.Count; i_q++)
            {
                streamWriter.Write("{0,4}|", i_q);
                for (int k = 0; k < this.Cgo[i_q].Count; k++)
                {
                    streamWriter.Write("{0,4}|", this.Cgo[i_q][k]);

                }
                streamWriter.WriteLine();
            }
            //PrintCFinals
        }

        //PrintFCgo == PrintFGo(Cfgo)
        public void PrintFCgo(StreamWriter streamWriter)
        {
            streamWriter.WriteLine();
            streamWriter.Write("q:a |");
            string akStr;
            for (int k = 0; k < this.classPartListPublic.Count; k++)
            {
                akStr = SubsetClass.CharToString(this.classPartListPublic.Keys[k]);
                ////streamWriter.Write("{0,3}|", this.classPartListPublic.Keys[k]);
                streamWriter.Write("{0,4}|", akStr);
            }
            streamWriter.WriteLine();

            for (int i_q = 0; i_q < this.Cfgo.Count; i_q++)
            {
                streamWriter.Write("{0,4}|", i_q);
                for (int k = 0; k < this.Cfgo[i_q].Count; k++)
                {
                    streamWriter.Write("{0,4}|", this.Cfgo[i_q][k]);

                }
                streamWriter.WriteLine();
            }
            //PrintFinals
        }

        //Print Public ClassPartList with <errorsDiagnostic==null>
        public void ShowClassPartList(StreamWriter streamWriter, SortedList<char, string> errorsDiagnostic)
        {
            char c;
            SubsetClass subsetClass;

            for (int i = 0; i < classPartListPublic.Count; i++)
            {

                c = classPartListPublic.Keys[i];
                subsetClass = classPartListPublic.Values[i];

                //1) Show (ci,Si)
                ShowAlphabetClass(streamWriter, i, c, subsetClass);

                //2)Show Diagnostics(ci,Si)
                if (errorsDiagnostic == null)
                    continue;

                if (errorsDiagnostic.ContainsKey(c))
                {
                    string sErr;
                    if (errorsDiagnostic.TryGetValue(c, out sErr))
                        streamWriter.WriteLine(sErr);
                    else
                        streamWriter.WriteLine("errorsDiagnostic.TryGetValue(c,out sErr)");
                };
            }

            streamWriter.WriteLine("*****");

        }
        private void ShowAlphabetClass(StreamWriter streamWriter, int i, char c, SubsetClass subsetClass)
        {
            streamWriter.WriteLine();

            //1) Show (ci,Si)
            //Show (ci)
            streamWriter.Write("Class({0}) : {1} = ", i, SubsetClass.CharToString(c));
            /*
                        if ((int)c != 0)
                            //streamWriter.Write(" Class({0,3:d}): {1}=", i, c);
                            streamWriter.Write(" Class({0,3:d}): {1}=", i, SubsetClass.CharToString(c));
                        else
                            streamWriter.Write(" Class({0,3:d}): {1}=", i, SubsetClass.CharToString(c));// "0x00"
            */
            //Show (Si)
            subsetClass.ToStreamWrite(streamWriter, false);//true
            //////streamWriter.WriteLine(); 

        }

        public void ToStreamCartesian(StreamWriter streamWriter)
        {
            //Print <automatonList Names
            //automatonList[i].nameExpression;
            string vectorAutomatonNames = "<";
            vectorAutomatonNames = vectorAutomatonNames + automatonList[0].nameExpression;
            for (int i = 1; i < this.automatonList.Count; i++)
                vectorAutomatonNames = vectorAutomatonNames + ", " + this.automatonList[i].nameExpression;
            vectorAutomatonNames=vectorAutomatonNames+">";
            //Line [1]
            if (this.bNoIntersection)
                streamWriter.WriteLine("These automata are separated!");
            else
                streamWriter.WriteLine("These automata are not separated!",vectorAutomatonNames);//Find Intersections
            //Line [2]
            streamWriter.WriteLine("Cartesian Product {0}", vectorAutomatonNames);
            //Lines [Cgo]
            PrintCgo(streamWriter);
            //Lines [CFinals]
            PrintCFinals(streamWriter);

            if (!this.bNoIntersection)
                FindPrintIntersectionsCFinals(streamWriter);
            //Lines [CActive and CErrors]
            PrintCActiveCErrors(streamWriter);
            //Lines [FCgo]
            PrintFCgo(streamWriter);

            //PrintPublicPartition
            streamWriter.WriteLine();
            streamWriter.WriteLine("Public Partition");
            ShowClassPartList( streamWriter, null);//SortedList<char, string> errorsDiagnostic
        }

    }
}
