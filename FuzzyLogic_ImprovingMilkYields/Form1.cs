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
            //richTextBox1.Text = string.Format("{0:0.00}\n", res);

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
                    ndf -= 0.1;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    textBox3.Text = string.Format("{0:0.00}", res);
                }
                else if (res < 22)
                {
                    ndf -= 0.5;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    textBox3.Text = string.Format("{0:0.00}", res);
                }
                else if (res > 33 && res < 40)
                {
                    ndf += 0.1;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
                    textBox3.Text = string.Format("{0:0.00}", res);
                }
                else if (res > 40)
                {
                    ndf += 0.5;
                    label7.Text = Convert.ToString(ndf);
                    GetData();
                    res = fuzz.Defuzzify();
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
            

            /*wind = new MembershipFunctionCollection();
            wind.Add(new MembershipFunction("OPPOSITE", -2.0, -0.5, -0.5, 0));
            wind.Add(new MembershipFunction("NEUTRAL", -0.1,0.9,0.9,1.1));
            wind.Add(new MembershipFunction("PARALLEL", 1.0,1.5,1.5,1.9));
            dWind = new LinguisticVariable("Wind", wind);

            altitude = new MembershipFunctionCollection();
            altitude.Add(new MembershipFunction("LOW", 0.0,1.0, 1.0, 2.3));
            altitude.Add(new MembershipFunction("OK", 2.0, 3.0, 3.0, 3.7));
            altitude.Add(new MembershipFunction("HIGH", 3.5, 4.3, 4.3, 4.9));
            dAltitude = new LinguisticVariable("Altitude", altitude);

            speed = new MembershipFunctionCollection();
            speed.Add(new MembershipFunction("SLOW", 0.0, 1.8,2.5,3.0));
            speed.Add(new MembershipFunction("FINE", 2.9,4.2,5.5,6.0));
            speed.Add(new MembershipFunction("FAST", 5.8,6.5,8.1,9.0));
            dSpeed = new LinguisticVariable("Speed", speed);

            land = new MembershipFunctionCollection();
            land.Add(new MembershipFunction("UNABLE", 0.0, 0.5, 0.5, 1.0));
            land.Add(new MembershipFunction("COULD", 0.8, 1.3, 1.5, 1.7));
            land.Add(new MembershipFunction("SHOULD", 1.6, 2.3, 2.5, 3.0));
            dLand = new LinguisticVariable("Land", land);*/



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
            /*dWind.InputValue = Convert.ToDouble(textBox2.Text);
            //dWind.Fuzzify("NEUTRAL");

            dAltitude.InputValue = Convert.ToDouble(textBox1.Text);
            //dAltitude.Fuzzify("OK");

            dSpeed.InputValue = Convert.ToDouble(textBox3.Text);
            //dSpeed.Fuzzify("FINE");*/

            NDF.InputValue = ndf;
            CBW.InputValue = cbw;

        }
    }
}
