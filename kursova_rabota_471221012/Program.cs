using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace kursova_rabota_471221012
{
    class Program
    {

        public class Stack<T>
        {
            const int MAX = 1000;
            int top;
            T[] stack = new T[MAX];
            public Stack()
            {
                top = -1;
            }
            public T Pop()
            {

                if (top < 0)
                {
                    Console.WriteLine("Stack Underflow");
                    return (T)Convert.ChangeType('X', typeof(T));
                }
                else
                {
                    T value = stack[top--];
                    return value;
                }
            }
            public bool Push(T data)
            {
                if (top >= MAX)
                {
                    Console.WriteLine("Stack Overflow");
                    return false;
                }
                else
                {
                    stack[++top] = data;
                    return true;
                }
            }
        }

        [Serializable]
        public class Node
        {
            public virtual bool value { get; set; }
            public virtual char data { get; set; }
            public virtual void SetValue()
            {

            }
            public virtual Node left { get; set; }
            public virtual Node right { get; set; }
        }
        [Serializable]
        public class OperandNode : Node
        {
            public override char data { get; set; }
            public override bool value { get; set; }
            public OperandNode(char ch)
            {
                data = ch;
            }
            public override Node right
            {
                get { return null; }
            }
            public override Node left
            {
                get { return null; }
            }

        }
        [Serializable]
        public class AndOperator : Node
        {
            public override char data
            {
                get
                {
                    return '&';
                }
            }

            public override Node left { get; set; }
            public override Node right { get; set; }
            private bool _value;
            public override void SetValue()
            {
                if ((left.value && !right.value) || (!left.value && right.value) ||
                    (!left.value && !right.value))
                {
                    _value = false;
                }
                else
                    _value = true;
            }
            public override bool value
            {
                get
                {
                    return _value;
                }
            }
        }
        [Serializable]
        public class OrOperator : Node
        {
            public override char data
            {
                get
                {
                    return '|';
                }
            }
            public override Node left { get; set; }
            public override Node right { get; set; }
            private bool _value;
            public override void SetValue()
            {
                if (!left.value && !right.value)
                {
                    _value = false;
                }
                else
                    _value = true;
            }
            public override bool value
            {
                get
                {
                    return _value;
                }
            }
        }
        [Serializable]
        public class NotOperator : Node
        {
            public override char data
            {
                get
                {
                    return '!';
                }
            }
            public override Node right { get; set; }
            public override Node left
            {
                get { return null; }
            }
            private bool _value;
            public override void SetValue()
            {
                if (right.value)
                {
                    _value = false;
                }
                else
                    _value = true;
            }
            public override bool value
            {
                get
                {
                    return _value;
                }
            }
        }



        public static bool isOperator(char ch)
        {
            if (ch == '&' || ch == '|' || ch == '!')
            {
                return true;
            }
            return false;
        }
        //public static bool isOperand(char ch, char[] izraz)
        //{
        //    bool flag = false;
        //    for (int i = 0; i < izraz.Length; i++)
        //    {
        //        if (ch == izraz[i])
        //        {
        //            flag = true;
        //        }
        //    }
        //    return flag;
        //}
        public static Node expressionTree(char[] izraz)
        {
            Stack<Node> st = new Stack<Node>();
            Node t1, t2, temp;

            for (int i = 0; i < izraz.Length; i++)
            {
                if (!isOperator(izraz[i]))
                {
                    temp = new OperandNode(izraz[i]);
                    st.Push(temp);
                }
                else if (izraz[i] == '&')
                {
                    t1 = st.Pop();
                    t2 = st.Pop();

                    temp = new AndOperator
                    {
                        left = t2,
                        right = t1
                    };


                    st.Push(temp);
                }
                else if (izraz[i] == '|')
                {
                    t1 = st.Pop();
                    t2 = st.Pop();

                    temp = new OrOperator
                    {
                        left = t2,
                        right = t1
                    };

                    st.Push(temp);
                }
                else if (izraz[i] == '!')
                {
                    t1 = st.Pop();

                    temp = new NotOperator
                    {
                        right = t1,
                    };

                    st.Push(temp);
                }
            }
            temp = st.Pop();
            return temp;
        }



        static string[] SplitI(string s, char[] symbols)
        {
            string[] s_splited = new string[50];
            int b = 0;
            char[] chh = new char[20];
            foreach (char ch in s)
            {
                bool is_symbol = false;
                foreach (char ch2 in symbols)
                {
                    if (ch == ch2)
                    {
                        is_symbol = true;
                    }
                }
                if (is_symbol == false)
                {
                    s_splited[b] = $"{s_splited[b]}{ch}";
                }
                else
                {
                    b++;
                }
            }

            List<string> sw = new List<string>();
            for (int i = 0; i < s_splited.Length; i++)
            {
                if (s_splited[i] != null)
                {
                    sw.Add(s_splited[i]);
                }
            }
            string[] ss_splited = new string[sw.Count];
            int k = 0;
            foreach (string str in sw)
            {
                ss_splited[k] = str;
                k++;
            }
            return ss_splited;
        }


        class InvalidValueException : Exception
        {
            public InvalidValueException(string m) : base(m)
            {

            }
        }

        [Serializable]
        public class Func
        {
            public string name { get; set; }
            public string[] Parameters { get; set; }
            public string izraz { get; set; }
            public string izraz_full { get; set; }

            public bool dr_func { get; set; }
            public string dr_func_name { get; set; }
            public string dr_func_izraz { get; set; }
            public Node tree { get; set; }
            public bool[,] table { get; set; }

        }
        static void InsertFunc(string fd, string[] f, List<Func> functions)
        {
            Func functionToBeInserted = new Func();
            char[] ch1 = { '(' };
            string[] ff = SplitI(f[1], ch1);
            functionToBeInserted.name = ff[0];

            char[] ch2 = { '(', ')' };
            char[] ch3 = { ',', ' ' };
            string[] fff = SplitI(fd, ch2);
            string[] par = SplitI(fff[1], ch3);
            functionToBeInserted.Parameters = new string[par.Length];
            for (int i = 0; i < functionToBeInserted.Parameters.Length; i++)
            {
                functionToBeInserted.Parameters[i] = par[i];
            }

            char[] ch4 = { '"' };
            string[] f_izraz = SplitI(fd, ch4);
            functionToBeInserted.izraz = f_izraz[1];

            char[] izraz_ch = functionToBeInserted.izraz.ToCharArray();
            char[] name_check = new char[15];
            for (int j = 0; j < izraz_ch.Length - 1; j ++)
            {
                if (izraz_ch[j] != ' ' && izraz_ch[j] != '&' && izraz_ch[j] != '|' && izraz_ch[j] != '!'
                    && izraz_ch[j + 1] != ' ' && izraz_ch[j + 1] != '!')
                {
                    int i = 0;
                    do
                    { 
                        name_check[i] = izraz_ch[j];
                        j++;
                        if (j > izraz_ch.Length)
                            break;
                        i++;
                    } while (izraz_ch[j] != '(');
                    break;
                }
                
            }

            if (functions != null&&name_check[0]!= '\x0000')
            {
                bool otkrit = false;
                foreach (Func fu in functions)
                {
                    char[] fu_check = fu.name.ToCharArray();
                    bool flag = true;
                    for (int i = 0; i < fu_check.Length; i++)
                    {
                        if (fu_check[i] != name_check[i])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        functionToBeInserted.dr_func = true;
                        functionToBeInserted.dr_func_name = fu.name;
                        functionToBeInserted.dr_func_izraz = fu.izraz;
                        otkrit = true;
                        break;
                    }
                }
                if(!otkrit)
                {
                    string greshen = "";
                    for(int i=0;i<name_check.Length;i++)
                    {
                        //if (name_check[i] == '\x0000')
                        //    break;
                        //else
                        //{
                            greshen =greshen + "" + name_check[i];
                        //}
                    }
                    throw new InvalidValueException($"Въвели сте грешно наименование на функцията в израза! Не същестмума {greshen}");
                }
            }

            for(int i=0;i<izraz_ch.Length;i++)
            {
                if ((izraz_ch[i] == ' ' || izraz_ch[i] == '&' || izraz_ch[i] == ','|| izraz_ch[i] == '('|| izraz_ch[i] == ')'||
                        izraz_ch[i] == '|' || izraz_ch[i] == '!')|| izraz_ch[i+1] != ' ')
                    continue;
                bool flag=false;
                for (int j = 0; j < functionToBeInserted.Parameters.Length; j++)
                {
                    if (izraz_ch[i] == char.Parse(functionToBeInserted.Parameters[j]))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    throw new InvalidValueException($"Грешка! Операторът \"{izraz_ch[i]}\" не е дефиниран!");
                }
            }


            functionToBeInserted.izraz_full = functionToBeInserted.izraz;

            if (functionToBeInserted.dr_func)
            {
                char[] drFuncName = functionToBeInserted.dr_func_name.ToCharArray();
                string nov = "";
                bool fl = false;
                foreach (char ch in functionToBeInserted.izraz)
                {

                    if (ch == ')' && fl)
                    {
                        fl = false;
                        continue;
                    }
                    if (ch != drFuncName[0] && !fl)
                    {
                        nov = $"{nov}{ch}";
                    }
                    else if (ch == drFuncName[0])
                    {
                        nov = $"{nov}{functionToBeInserted.dr_func_izraz}";
                        fl = true;
                    }
                }
                functionToBeInserted.izraz_full = nov;
            }

            int br = 0;
            foreach (char ch in functionToBeInserted.izraz_full)
            {
                if (ch != ' ')
                {
                    br++;
                }
            }
            char[] a = new char[br];
            br = 0;
            foreach (char ch in functionToBeInserted.izraz_full)
            {
                if (ch != ' ')
                {
                    a[br] = ch;
                    br++;
                }
            }
            functionToBeInserted.tree = expressionTree(a);
            functionToBeInserted.table = Table_res(functionToBeInserted);

            functions.Add(functionToBeInserted);

            using (StreamWriter writer = new StreamWriter("memory.txt", true, Encoding.GetEncoding("UTF-8")))
            {
                string zapis = $"{functionToBeInserted.name}: \"{functionToBeInserted.izraz_full}\"";
                writer.WriteLine(zapis);
            }


            IFormatter formatter = new BinaryFormatter();
            using (var fs = new FileStream("memory2", FileMode.Create))
            {
                formatter.Serialize(fs, functions);
            }

        }

        public static bool[,] Table_res(Func currentFunc)
        {
            
            bool[,] tablica = new bool[(int)Math.Pow(2.0, currentFunc.Parameters.Length), currentFunc.Parameters.Length + 1];
            int period = tablica.GetLength(0) / 2;
            int col = 0;
            while (period >= 1)
            {
                int red = 0;
                while (red < tablica.GetLength(0) - 1)
                {
                    bool flag = false;
                    for (int broiach = 1; broiach <= period; broiach++)
                    {
                        tablica[red, col] = flag;
                        red++;
                    }
                    flag = true;
                    for (int broiach = 1; broiach <= period; broiach++)
                    {
                        tablica[red, col] = flag;
                        red++;
                    }
                }
                col++;
                period = period / 2;
            }
            for (int r = 0; r < tablica.GetLength(0); r++)
            {
                tablica[r, currentFunc.Parameters.Length] = S_f(currentFunc, tablica, r);
            }
            return tablica;
        }

        private static bool res = false;
        public static bool Solve(Node tree)
        {
            if (tree.left == null && tree.right == null)
            {
                res = tree.value;
                return tree.value;
            }

            else if (tree.data == '!')
            {
                //return !tree.right.value;
                bool rightTt = Solve(tree.right);
                res = !rightTt;
                return !rightTt;
            }
            else
            {
                bool leftT = Solve(tree.left);
                bool rightT = Solve(tree.right);

                if (tree.data == '&')
                {
                    res = leftT & rightT;
                    return leftT & rightT;
                }
                else if (tree.data == '|')
                {
                    res = leftT | rightT;
                    return leftT | rightT;
                }

            }
            return res;
        }

        public static Node FindNode(Node begin,char par)
        {
            if (begin == null)
                return null;
            if (par == begin.data)
                return begin;
            
            return
                FindNode(begin.left, par) ??
                FindNode(begin.right, par);
        }

        public static bool SolveFunction(string name_f,string fd, List<Func> functions)
        {
            Func currentFunc = new Func();
            foreach(Func f in functions)
            {
                if(f.name==name_f)
                {
                    currentFunc = f;
                    break;
                }
            }

            int r = 0;
            char[] ch = { '(', ')' };
            string[] p = SplitI(fd,ch);
            foreach(char c in p[1])
            {
                if(c!=','&& c != ' ')
                {
                    r++;
                }
            }
            char[] par_val = new char[r];
            r = 0;
            foreach (char c in p[1])
            {
                if (c != ',' && c != ' ')
                {
                    par_val[r] = c;
                    r++;
                }
            }

            for (int i = 0; i < currentFunc.Parameters.Length; i++)
            {
                Node n=FindNode(currentFunc.tree,char.Parse(currentFunc.Parameters[i]));
                bool sign = false;
                if(par_val[i]=='1')
                {
                    sign = true;
                }
                n.value = sign;
            }

            return Solve(currentFunc.tree);
        }

        public static void ALL(string name_f,List<Func> functions)
        {
            Func currentFunc = new Func();
            foreach (Func f in functions)
            {
                if (f.name == name_f)
                {
                    currentFunc = f;
                    break;
                }
            }

            int v = 0;
            while(v<currentFunc.Parameters.Length)
            {
                Console.Write($" {currentFunc.Parameters[v]} |");
                v++;
            }
            Console.Write(" res ");
            Console.WriteLine();

            

            for (int j=0;j<currentFunc.table.GetLength(0);j++)
            {
                for(int k=0;k< currentFunc.table.GetLength(1);k++)
                {
                    int res;
                    if (currentFunc.table[j, k])
                    {
                        res = 1;
                    }
                    else
                        res = 0;
                    Console.Write($" {res} ");
                    if(k< currentFunc.table.GetLength(1)-1)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
            }
        }

        public static bool S_f(Func currentfunction, bool[,] tablica,int r)
        {
            int col = 0;
            bool[] t = new bool[tablica.GetLength(1)-1];
            while(col<=tablica.GetLength(1)-2)
            {
                t[col] = tablica[r, col];
                col++;
            }

            Node tree2 = currentfunction.tree;

            for (int i = 0; i < currentfunction.Parameters.Length; i++)
            {
                Node n = FindNode(tree2, char.Parse(currentfunction.Parameters[i]));
                n.value = t[i];
            }
            return Solve(tree2);
        }

        public static int[,] TheRestOfTheTable(int[,] ttable,char[] chh)
        {
            int red = 1;
            string linee = Console.ReadLine();

            while(!linee.Equals(""))
            {
                string[] lineee = SplitI(linee, chh);
                for(int col=0;col<ttable.GetLength(1);col++)
                {
                    ttable[red,col]= Convert.ToInt32(lineee[col]);
                }
                red++;
                linee = Console.ReadLine();
            }
            return ttable;
        }

        public static void Finding(int[,] tableToBeFound, List<Func> functions)
        {

            bool[,] tableToBeFoundBool = new bool[tableToBeFound.GetLength(0), tableToBeFound.GetLength(1)];
            for (int red = 0; red < tableToBeFoundBool.GetLength(0); red++)
            {
                for(int col=0;col< tableToBeFoundBool.GetLength(1); col++)
                {
                    if(tableToBeFound[red,col]==1)
                    {
                        tableToBeFoundBool[red, col] = true;
                    }
                }
            }

            foreach(Func f in functions)
            {
                if(f.table.GetLength(0)==tableToBeFound.GetLength(0))
                {
                    bool flag=true;
                    for(int red=0;red<tableToBeFoundBool.GetLength(0);red++)
                    {
                        for(int col = 0; col < tableToBeFoundBool.GetLength(1); col++)
                        {
                            if(f.table[red,col]!=tableToBeFoundBool[red,col])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag)
                            break;
                    }

                    if(flag)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{f.izraz_full}");
                        break;
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            List<Func> functions = new List<Func>();

            if (File.Exists("memory2"))
            {

                IFormatter formatter = new BinaryFormatter();

                using (var fs = new FileStream("memory2", FileMode.Open))
                {
                    functions = (List<Func>)formatter.Deserialize(fs);
                }
            }

            string fd = Console.ReadLine();
            char[] ch = { ' ','(' };
            string[] f = SplitI(fd, ch);
            string name_f = f[1];

            switch (f[0])
            {
                case "DEFINE":
                    InsertFunc(fd, f, functions);
                    break;
                case "SOLVE":
                    bool result=SolveFunction(name_f, fd, functions);
                    if(result)
                    {
                        Console.WriteLine($"Result: 1");
                    }
                    else
                    {
                        Console.WriteLine($"Result: 0");
                    }
                    break;
                case "ALL":
                    ALL(name_f, functions);
                    break;
                case "FIND":
                    char[] chh = { ',', ':',';', ' ' };
                    string[] line = SplitI(f[1],chh);
                    int[,] ttable = new int[(int)Math.Pow(2,line.Length-1),line.Length];
                    for(int i=0;i<line.Length;i++)
                    {
                        ttable[0, i] = Convert.ToInt32(line[i]);
                    }
                    int[,] tableToBeFound= TheRestOfTheTable(ttable,chh);
                    Finding(tableToBeFound, functions);
                    break;
            }


            //char[] symbols ={ ' ', ',' };

            ////SplitI("i love,you", symbols);
            //string[] x = SplitI("i love,you", symbols);
            //for (int i = 0; i < x.Length; i++)
            //{
            //    if(x[i]!=null)
            //    {
            //        Console.WriteLine(x[i]);
            //    }
            //}
        }

    }
}
