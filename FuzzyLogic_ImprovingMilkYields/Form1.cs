using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotFuzzy;

namespace FuzzyLogic
{
    public partial class Form1 : Form
    {
        /*LinguisticVariable dSpeed, dAltitude, dWind, dLand;
        MembershipFunctionCollection wind, altitude, speed, land;*/
        LinguisticVariable NDF, CBW, DMI;

        FuzzyEngine fuzz;

        double ndf, cbw, res;
        int check = 0;

        //public static double Altitude, Speed;

        private async void button1_Click(object sender, EventArgs e)
        {

            ndf = Convert.ToDouble(trackBar1.Value);
            cbw = Convert.ToDouble(trackBar2.Value);
            fuzz = new FuzzyEngine();
            AddMembers();
            SetRules();
            res = fuzz.Defuzzify();

            do
            {
                if (res >= 28 && res <= 33)
                {
                    label7.Text = Convert.ToString(ndf);
                    textBox3.Text = string.Format("{0:0.00}", res);
                    check = 1;
                }
                else if (res < 28 && res > 22)
                {
                    ndf += 0.1;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    check = 0;
                    textBox3.Text = string.Format("{0:0.00}", res);
                }
                else if (res < 22)
                {
                    ndf += 0.5;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    check = 0;
                    textBox3.Text = string.Format("{0:0.00}", res);
                }
                else if (res > 33 && res < 40)
                {
                    ndf -= 0.1;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    check = 0;
                    textBox3.Text = string.Format("{0:0.00}", res);
                }
                else if (res > 40)
                {
                    ndf -= 0.5;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    check = 0;
                    textBox3.Text = string.Format("{0:0.00}", res);
                }

                await Task.Delay(200);
                
            } while (check != 1);


        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label5.Text = trackBar2.Value.ToString();
        }

        public Form1()
        {
            InitializeComponent();
        }

        public void AddMembers()
        {
            MembershipFunctionCollection ndf = new MembershipFunctionCollection();
            ndf.Add(new MembershipFunction("SMALL", 40.0, 47, 47, 48));
            ndf.Add(new MembershipFunction("MEDIUM", 46, 60, 60, 69));
            ndf.Add(new MembershipFunction("LARGE", 65, 72, 72, 74));
            NDF = new LinguisticVariable("NDF", ndf);



            MembershipFunctionCollection cbw = new MembershipFunctionCollection();
            cbw.Add(new MembershipFunction("SMALL", 1020, 1250, 1250, 1450));
            cbw.Add(new MembershipFunction("MEDIUM", 1390, 1550, 1550, 1750));
            cbw.Add(new MembershipFunction("LARGE", 1680, 1820, 1820, 1890));
            CBW = new LinguisticVariable("CBW", cbw);


            MembershipFunctionCollection dmi = new MembershipFunctionCollection();
            dmi.Add(new MembershipFunction("DAWL", 14, 18, 18, 22)); //Decrease a whole lot
            dmi.Add(new MembershipFunction("DAGA", 20, 24, 24, 29)); //Decrease a good amount
            dmi.Add(new MembershipFunction("ENOUGH", 27, 31, 31, 34));
            dmi.Add(new MembershipFunction("IAGA", 32, 38, 38, 42)); //Increase a good amount
            dmi.Add(new MembershipFunction("IAWL", 40,47,47,52)); ; //Increase a whole lot
            DMI = new LinguisticVariable("DMI", dmi);

        }

        public void SetRules()
        {
            fuzz.LinguisticVariableCollection.Add(CBW);
            fuzz.LinguisticVariableCollection.Add(NDF);
            fuzz.LinguisticVariableCollection.Add(DMI);
            fuzz.Consequent = "DMI";

            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS SMALL) AND (NDF IS SMALL) THEN DMI IS ENOUGH"));
            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS MEDIUM) AND (NDF IS SMALL) THEN DMI IS DAGA"));
            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS LARGE) AND (NDF IS SMALL) THEN DMI IS DAWL"));

            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS SMALL) AND (NDF IS MEDIUM) THEN DMI IS IAGA"));
            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS MEDIUM) AND (NDF IS MEDIUM) THEN DMI IS ENOUGH"));
            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS LARGE) AND (NDF IS MEDIUM) THEN DMI IS DAGA"));

            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS SMALL) AND (NDF IS LARGE) THEN DMI IS IAWL"));
            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS MEDIUM) AND (NDF IS LARGE) THEN DMI IS IAGA"));
            fuzz.FuzzyRuleCollection.Add(new FuzzyRule("IF (CBW IS LARGE) AND (NDF IS LARGE) THEN DMI IS ENOUGH"));

            GetData();
        }


        public void GetData()
        {
            NDF.InputValue = ndf;
            CBW.InputValue = cbw;

        }
    }
}
