﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CommonData.HIRATA;
using KgsCommon;
using BaseAp;

namespace UI
{
    public partial class user_SamplingSlot : UserControl
    {
        public user_SamplingSlot()
        {
            InitializeComponent();
            dataGridView2.AutoGenerateColumns = false;
            UiForm.AddUiObjToEnableList(button5, UiForm.enumGroup.Group4 );
            UiForm.AddUiObjToEnableList(button6, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(button4, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(button1, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(button2, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(button3, UiForm.enumGroup.Group4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (CheckData(CommonData.HIRATA.DataEidtAction.Add))
            {
                SendSamplingDataAction(CommonData.HIRATA.DataEidtAction.Add);
                
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private bool CheckData(CommonData.HIRATA.DataEidtAction m_Action)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool is_ok = true;
            string id = textBox_id.Text.Trim();
            bool method_slot = radioButton_byslot.Checked;
            bool method_period = radioButton_byPeriod.Checked;
            bool front = radioButton_front.Checked;
            bool back = radioButton_back.Checked;
            string des = textBox_description.Text.Trim();
            string  n = textBox_n.Text.Trim();
            string  m = textBox_m.Text.Trim();

            if( id=="" || (!Regex.Match(textBox_id.Text, @"\d+").Success)) //|| Regex.Match(cv_TxRecipeId.Text , @"^0" , RegexOptions.IgnoreCase).Succes
            { 
                UI.CommonStaticData.PopForm("The id should be digital number!", true, false);
                return false;
            }
            if( !method_period && !method_slot)
            {
                UI.CommonStaticData.PopForm("The method can't empty !!!", true, false);
                return false;
            }
            if (method_period)
            {
                if( (n == "") || (m == "") || (!Regex.Match(n, @"\d+").Success) || (!Regex.Match(m, @"\d+").Success))
                {
                    UI.CommonStaticData.PopForm("The N and M can't be empty and should be digital !!!", true, false);
                    return false;
                }
                if (!front && !back)
                {
                    UI.CommonStaticData.PopForm("The fron and back can't be empty !!!", true, false);
                    return false;
                }
            }

            int tmp_no = -1;
            if (int.TryParse(textBox_id.Text, out tmp_no))
            {
                bool is_exist = UiForm.cv_SamplingData.cv_SamplingList.Exists(x => x.PNo == tmp_no);

                if (m_Action == CommonData.HIRATA.DataEidtAction.Add && is_exist)
                {
                    UI.CommonStaticData.PopForm("The Sampling item already exist!!!", true, false);
                    is_ok = false;
                }
                else if (m_Action == CommonData.HIRATA.DataEidtAction.Del && !is_exist)
                {
                    UI.CommonStaticData.PopForm("The Sampling item not found!!!", true, false);
                    is_ok = false;
                }
                else if (m_Action == CommonData.HIRATA.DataEidtAction.Edit && !is_exist)
                {
                    UI.CommonStaticData.PopForm("The Sampling item not found!!!", true, false);
                    is_ok = false;
                }
            }
            else
            {
                UI.CommonStaticData.PopForm("The id should be digital number !!!", true, false);
                return is_ok;
            }

            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return is_ok;
        }
        private void SendSamplingDataAction(CommonData.HIRATA.DataEidtAction m_Action)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDSamplingDataAction obj = new CommonData.HIRATA.MDSamplingDataAction();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etNotify;

            switch (m_Action)
            {
                case CommonData.HIRATA.DataEidtAction.Add:
                    obj.PAction = CommonData.HIRATA.DataEidtAction.Add;
                    break;
                case CommonData.HIRATA.DataEidtAction.Edit:
                    obj.PAction = CommonData.HIRATA.DataEidtAction.Edit;
                    break;
                case CommonData.HIRATA.DataEidtAction.Del:
                    obj.PAction = CommonData.HIRATA.DataEidtAction.Del;
                    break;

            };

            bool is_ok = true;
            string id = textBox_id.Text.Trim();
            bool method_slot = radioButton_byslot.Checked;
            bool method_period = radioButton_byPeriod.Checked;
            bool front = radioButton_front.Checked;
            bool back = radioButton_back.Checked;
            string des = textBox_description.Text.Trim();
            string n = textBox_n.Text.Trim();
            string m = textBox_m.Text.Trim();

            CommonData.HIRATA.SamplingIem tmp = new CommonData.HIRATA.SamplingIem();
            tmp.PNo = int.Parse(textBox_id.Text.Trim());
            tmp.PTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            tmp.PDesription = textBox_description.Text.Trim();
            tmp.PMethod = getmethod();
            if(tmp.PMethod == SamplingMethod.BySlot)
            {
                tmp.PHitList = GetHitList();
            }
            else if(tmp.PMethod == SamplingMethod.ByPeriod)
            {
                tmp.PFromfornt = isFront();
                tmp.PM = int.Parse(m);
                tmp.PN = int.Parse(n);
            }
            obj.SamplingDatas.Add(tmp);
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDSamplingDataAction).Name, obj , KParseObjToXmlPropertyType.Field);
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private SamplingMethod getmethod()
        {
            SamplingMethod rtn = SamplingMethod.None;
            bool method_slot = radioButton_byslot.Checked;
            bool method_period = radioButton_byPeriod.Checked;
            if(method_period && !method_slot)
            {
                rtn = SamplingMethod.ByPeriod;
            }
            else if(!method_period && method_slot)
            {
                rtn = SamplingMethod.BySlot;
            }
            return rtn;
        }
        private bool isFront()
        {
            bool rtn = false;
            bool front = radioButton_front.Checked;
            bool back = radioButton_back.Checked;
            if (front && !back)
            {
                rtn = true;
            }
            else if (!front && back)
            {
                rtn = false;
            }
            return rtn;
        }
        private bool IsHasCstLDCM()
        {
            bool has_cst = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                if (UiForm.GetPort(i).cv_Data.PPortStatus == PortStaus.LDCM)
                {
                    //log += "But Port : " + i.ToString() + " has Cst !!!" + Environment.NewLine;
                    has_cst = true;
                    break;
                }
            }
            return has_cst;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string sampling_id = textBox_id.Text.Trim();
            string log = "User press sampling item modify : " + sampling_id + Environment.NewLine;
            RecipeItem recipe = null;
            if (UiForm.cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe != null)
                {
                    if (recipe.PSampling.ToString().Trim() == sampling_id.Trim())
                    {
                        log += " Current recipe's sampling : " + recipe.PSampling + Environment.NewLine;
                        bool has_cst = false;
                        has_cst = IsHasCstLDCM();
                        if (has_cst)
                        {
                            CommonStaticData.PopForm("Cant't modify Current recipe.", true, false);
                            if (!string.IsNullOrEmpty(log))
                            {
                                UiForm.WriteLog(LogLevelType.General, log);
                            }
                            return;
                        }
                    }
                }
            }
            if (CheckData(CommonData.HIRATA.DataEidtAction.Edit))
            {
                SendSamplingDataAction(CommonData.HIRATA.DataEidtAction.Edit);
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string sampling_id = textBox_id.Text.Trim();
            RecipeItem recipe = null;
            if (UiForm.cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe != null)
                {
                    if (recipe.PSampling.ToString().Trim() == sampling_id.Trim())
                    {
                        bool has_cst = false;
                        has_cst = IsHasCstLDCM();
                        if (has_cst)
                        {
                            CommonStaticData.PopForm("Cant't delete Current recipe's sampling item. beacause has Port LDCM", true, false);
                            return;
                        }
                    }
                }
            }
            if (CheckData(CommonData.HIRATA.DataEidtAction.Del))
            {
                SendSamplingDataAction(CommonData.HIRATA.DataEidtAction.Del);
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int slot_number = checkedListBox1.Items.Count;
            for(int i=0; i<slot_number;i++ )
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private List<int> GetHitList()
        {
            List<int> rtn = new List<int>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string tmp = checkedListBox1.Items[i].ToString();
                    Match match = Regex.Match(tmp, @"\d+");
                    if (match.Success)
                    {
                        string str_slot = match.Value;
                        int slot = -1;
                        if (int.TryParse(str_slot, out slot))
                        {
                            rtn.Add(slot);
                        }
                    }
                }
            }
            return rtn;
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (e.RowIndex == -1) return;
            string select_str = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            int index = UiForm.cv_SamplingData.cv_SamplingList.FindIndex(x => x.PNo.ToString() == select_str);
            if (-1 != index)
            {
                clear();
                SamplingIem sampling = UiForm.cv_SamplingData.cv_SamplingList[index];
                if(sampling.PMethod == SamplingMethod.ByPeriod)
                {
                    radioButton_byPeriod.Checked = true;
                }
                else if(sampling.PMethod == SamplingMethod.BySlot)
                {
                    radioButton_byslot.Checked = true;
                }

                textBox_id.Text = UiForm.cv_SamplingData.cv_SamplingList[index].PNo.ToString();
                textBox_description.Text = UiForm.cv_SamplingData.cv_SamplingList[index].PDesription;

                if (sampling.PMethod == SamplingMethod.BySlot)
                {
                    //clean all.
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                    //set true.
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        string tmp = checkedListBox1.Items[i].ToString();
                        Match match = Regex.Match(tmp, @"\d+");
                        if (match.Success)
                        {
                            string str_slot = match.Value;
                            int slot = -1;
                            if (int.TryParse(str_slot, out slot))
                            {
                                if (UiForm.cv_SamplingData.cv_SamplingList[index].PHitList.Contains(slot))
                                {
                                    checkedListBox1.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                }
                else if(sampling.PMethod == SamplingMethod.ByPeriod)
                {
                    if(sampling.PFromfornt)
                    {
                        radioButton_front.Checked = true;
                        radioButton_back.Checked = false;
                    }
                    else
                    {
                        radioButton_front.Checked = false;
                        radioButton_back.Checked = true;
                    }
                    textBox_n.Text = sampling.PN.ToString();
                    textBox_m.Text = sampling.PM.ToString();
                }
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void clear()
        {
            /*
            bool is_ok = true;
            string id = textBox_id.Text.Trim();
            bool method_slot = radioButton_byslot.Checked;
            bool method_period = radioButton_byPeriod.Checked;
            bool front = radioButton_front.Checked;
            bool back = radioButton_back.Checked;
            string des = textBox_description.Text.Trim();
            string n = textBox_n.Text.Trim();
            string m = textBox_m.Text.Trim();
            */

            textBox_id.Text = "";
            radioButton_byslot.Checked = false;
            radioButton_byPeriod.Checked = false;
            radioButton_front.Checked = false;
            radioButton_back.Checked = false;
            textBox_description.Text = "";
            textBox_n.Text = "";
            textBox_m.Text = "";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string tmp = checkedListBox1.Items[i].ToString();
                Match match = Regex.Match(tmp, @"\d+");
                if (match.Success)
                {
                    string str_slot = match.Value;
                    int slot = -1;
                    if (int.TryParse(str_slot, out slot))
                    {
                        if (slot % 2 == 0)
                        {
                            checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string tmp = checkedListBox1.Items[i].ToString();
                Match match = Regex.Match(tmp, @"\d+");
                if (match.Success)
                {
                    string str_slot = match.Value;
                    int slot = -1;
                    if (int.TryParse(str_slot, out slot))
                    {
                        if (slot % 2 == 1)
                        {
                            checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }
    }
}
