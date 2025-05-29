using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
    internal partial class RecipeSetting : UserControl
    {
        Dictionary<CommonData.HIRATA.OdfFlow, string> cv_FlowDescription = new Dictionary<CommonData.HIRATA.OdfFlow, string>();
        public RecipeSetting()
        {
            InitializeComponent();
            cv_RecipeDataView.AutoGenerateColumns = false;
            LoadFlowName();
            InitFlowDescription();
            UiForm.AddUiObjToEnableList(btn_Add , UiForm.enumGroup.Group4 );
            UiForm.AddUiObjToEnableList(btn_Modify, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(btn_Del, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(btn_CurRecipe, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cb_NeedGlass, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cb_waferPutUp, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cb_FlipToUv, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cb_backToLd, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cb_Rework, UiForm.enumGroup.Group4);
        }
        public void setNoNeedUiVisiable(bool m_canSee)
        {
            cb_NeedGlass.Visible = m_canSee;
            cb_waferPutUp.Visible = m_canSee;
            cb_FlipToUv.Visible = m_canSee;
            cb_backToLd.Visible = m_canSee;
        }
        private void InitFlowDescription()
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow1_1, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> IJP -> Aligner -> VAS(Low)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow1_1] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> Flip(TopWaferPut) -> VAS(Up)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow1_1] += "Combination : VAS(Low) -> UV -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow1_2, "Wafer :  Port -> Aligner -> Buffer -> SDP -> Aligner -> IJP -> VAS(Low)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow1_2] += "Glass : Port -> Aligner -> Buffer -> Flip(TopWaferPut) -> VAS(Up)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow1_2] += "Combination : VAS(Low) -> UV -> Unload Port";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow2_1, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> IJP -> UV -> ULD");

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow2_2, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> Aligner -> IJP -> UV -> ULD");

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow2_3, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> Aligner -> IJP -> UV -> LD");

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow3, "Wafer :  Port(LP5/LP6) -> UV -> ULD");

            /*
            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.FLow4_1, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP ->\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.FLow4_1] += "1: AOI(RP) -> SDP -> IJP -> AlignerVAS ->(Low)\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.FLow4_1] += "2: AOI(OK) -> IJP -> Aligner VAS ->(Low)\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.FLow4_1] += "3: IJP -> VAS ->(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.FLow4_1] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> Flip(TopWaferPut) -> VAS(Up)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.FLow4_1] += "Combination : VAS(Low) -> UV -> Unload Port";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow4_2, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP ->\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow4_2] += "1: AOI(RP) -> SDP -> Aligner -> IJP -> Aligner -> VAS ->(Low)\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow4_2] += "2: AOI(OK) -> IJP -> Aligner -> VAS ->(Low)\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow4_2] += "3: Aligner -> IJP -> Aligner -> VAS ->(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow4_2] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> Flip(TopWaferPut) -> VAS(Up)\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow4_2] += "Combination : VAS(Low) -> UV -> Unload Port";
            */

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow5_1, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> IJP -> Aligner -> Flip(TopWaferPut)-> VAS(Up)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow5_1] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow5_1] += "Combination : VAS(Low) -> UV (UV Flip) -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow5_2, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> IJP -> Aligner -> Flip(TopWaferPut)-> VAS(Up)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow5_2] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow5_2] += "Combination : VAS(Low) -> Flip(TopWaferPut) -> UV -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow6_1, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> IJP -> Aligner -> VAS(Low)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow6_1] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Up)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow6_1] += "Combination : VAS(Low) -> UV -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow6_2, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> Aligner -> IJP -> VAS(Low)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow6_2] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Up)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow6_2] += "Combination : VAS(Low) -> UV -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow6_3, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> IJP -> VAS(Low)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow6_3] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Up)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow6_3] += "Combination : VAS(Low) -> UV -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow7_2, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> IJP -> Aligner -> VAS(Up)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow7_2] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow7_2] += "Combination : VAS(Low) -> UV (UV Flip) -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow7_3, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> Aligner -> IJP -> VAS(Up)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow7_3] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow7_3] += "Combination : VAS(Low) -> UV (UV Flip) -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow7_4, "Wafer :  Port(LP5/LP6) -> Aligner -> Buffer -> SDP -> AOI -> IJP -> VAS(Up)\n\n");
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow7_4] += "Glass : Port(LP3/LP4) -> Aligner -> Buffer -> VAS(Low)\n\n";
            cv_FlowDescription[CommonData.HIRATA.OdfFlow.Flow7_4] += "Combination : VAS(Low) -> UV -> Unload Port(ULD1/ULD2)";

            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow8_1, "Combination :  Port(LP5/LP6) -> Aligner -> Buffer -> AOI -> UD");
            cv_FlowDescription.Add(CommonData.HIRATA.OdfFlow.Flow8_2, "Combination :  Port(UP1/UP2) -> Aligner -> Buffer -> AOI -> UD");
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void LoadFlowName()
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            List<string> flows = Enum.GetNames(typeof(CommonData.HIRATA.OdfFlow)).ToList<string>();
            flows.Sort();

            foreach (string flow in flows)
            {
                if (!Regex.Match(flow, @"4_", RegexOptions.IgnoreCase).Success)
                {
                    cb_Flow.Items.Add(flow);
                }
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void cv_RecipeDataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Cells must to 0. 
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (e.RowIndex == -1) return;
            string select_str = cv_RecipeDataView.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            int index = UiForm.cv_Recipes.cv_RecipeList.FindIndex(x => x.cv_Id == select_str);
            if (-1 != index)
            {
                cv_TxRecipeId.Text = UiForm.cv_Recipes.cv_RecipeList[index].PId;
                cb_Flow.Text = UiForm.cv_Recipes.cv_RecipeList[index].PFlow.ToString();
                cv_TxWaferOCR.Text = UiForm.cv_Recipes.cv_RecipeList[index].PWaferVASDegree.ToString();
                cv_TxWaferVas.Text = UiForm.cv_Recipes.cv_RecipeList[index].PWaferIJPDegree.ToString();
                cv_TxGlassVas.Text = UiForm.cv_Recipes.cv_RecipeList[index].PGlassVASDegree.ToString();
                lbl_RecipeDescription.Text = UiForm.cv_Recipes.cv_RecipeList[index].PDecription;
                cb_samplingRate.Text = UiForm.cv_Recipes.cv_RecipeList[index].PSampling.ToString();
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private bool CheckData(CommonData.HIRATA.DataEidtAction m_Action)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool is_ok = true;
            if (cv_TxRecipeId.Text == "") //|| Regex.Match(cv_TxRecipeId.Text , @"^0" , RegexOptions.IgnoreCase).Success)
            { is_ok = false; }
            else if (cb_Flow.Text == "")
            { is_ok = false; }
            else if (cv_TxWaferOCR.Text == "")
            { is_ok = false; }
            else if (cv_TxWaferVas.Text == "")
            { is_ok = false; }
            else if (cv_TxGlassVas.Text == "")
            { is_ok = false; }
            if (!is_ok)
            {
                UI.CommonStaticData.PopForm("The Recipe data can't empty !!!", true, false);
                return is_ok;
            }
            if (Regex.Match(cv_TxRecipeId.Text.Trim(), @"[^0-9]|^0", RegexOptions.IgnoreCase).Success)
            {
                UI.CommonStaticData.PopForm("The Recipe Id only digital !!!", true, false);
                is_ok = false;
                return is_ok;
            }
            int int_id = Convert.ToInt32(cv_TxRecipeId.Text.Trim());
            if (int_id <= 0 || int_id > 999)
            {
                UI.CommonStaticData.PopForm("The Recipe Id over range !!!", true, false);
                is_ok = false;
                return is_ok;
            }

            bool is_exist = UiForm.cv_Recipes.cv_RecipeList.Exists(x => x.cv_Id == cv_TxRecipeId.Text.Trim());

            if (m_Action == CommonData.HIRATA.DataEidtAction.Add && is_exist)
            {
                UI.CommonStaticData.PopForm("The Recipe already exist!!!", true, false);
                is_ok = false;
            }
            else if (m_Action == CommonData.HIRATA.DataEidtAction.Del && !is_exist)
            {
                UI.CommonStaticData.PopForm("The Recipe not found!!!", true, false);
                is_ok = false;
            }
            else if (m_Action == CommonData.HIRATA.DataEidtAction.Edit && !is_exist)
            {
                UI.CommonStaticData.PopForm("The Recipe not found!!!", true, false);
                is_ok = false;
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return is_ok;
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (CheckData(CommonData.HIRATA.DataEidtAction.Add))
            {
                SendRecieAction(CommonData.HIRATA.DataEidtAction.Add);
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void btn_Modify_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string recipe_id = cv_TxRecipeId.Text.Trim();
            string log = "User press recipe modify : " + recipe_id + Environment.NewLine;
            RecipeItem recipe = null;
            if (UiForm.cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe != null)
                {
                    if (recipe.PId == recipe_id)
                    {
                        log += " Current recipe : " + recipe.PId + Environment.NewLine;
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
                SendRecieAction(CommonData.HIRATA.DataEidtAction.Edit);
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void btn_Del_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string recipe_id = cv_TxRecipeId.Text.Trim();
            RecipeItem recipe = null;
            if (UiForm.cv_Recipes.GetCurRecipe(out recipe))
            {
                if (recipe != null)
                {
                    if (recipe.PId == recipe_id)
                    {
                        CommonStaticData.PopForm("Cant't delete Current recipe.", true, false);
                        return;
                    }
                }
            }
            if (CheckData(CommonData.HIRATA.DataEidtAction.Del))
            {
                SendRecieAction(CommonData.HIRATA.DataEidtAction.Del);
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void SendRecieAction(CommonData.HIRATA.DataEidtAction m_Action)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDRecipeAction obj = new CommonData.HIRATA.MDRecipeAction();
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

            CommonData.HIRATA.RecipeItem tmp = new CommonData.HIRATA.RecipeItem();
            tmp.PId = cv_TxRecipeId.Text.Trim();
            tmp.PFlow = (CommonData.HIRATA.OdfFlow)Enum.Parse(typeof(CommonData.HIRATA.OdfFlow), cb_Flow.Text.Trim());

            if (tmp.PFlow == OdfFlow.Flow1_1 || tmp.PFlow == OdfFlow.Flow1_2 || tmp.PFlow == OdfFlow.FLow4_1 || tmp.PFlow == OdfFlow.Flow4_2 || tmp.PFlow == OdfFlow.Flow5_1 || tmp.PFlow == OdfFlow.Flow5_2 ||
                tmp.PFlow == OdfFlow.Flow6_1 || tmp.PFlow == OdfFlow.Flow6_2 || tmp.PFlow == OdfFlow.Flow6_3 || tmp.PFlow == OdfFlow.Flow7_2 || tmp.PFlow == OdfFlow.Flow7_3 || tmp.PFlow == OdfFlow.Flow7_4)
            {
                tmp.PVasNeedGlass = true;
            }
            else
            {
                tmp.PVasNeedGlass = false;
            }
            if(tmp.PFlow == OdfFlow.Flow5_1 || tmp.PFlow == OdfFlow.Flow5_2 || tmp.PFlow == OdfFlow.Flow7_2 || tmp.PFlow == OdfFlow.Flow7_3 || tmp.PFlow == OdfFlow.Flow7_4 )
            {
                tmp.PWaferPutUp = true;
            }
            else
            {
                tmp.PWaferPutUp = false;
            }    
            if(tmp.PFlow == OdfFlow.Flow5_1)
            {
                tmp.PFlipToUv = true;
            }
            else
            {
                tmp.PFlipToUv = false;
            }

            if(tmp.PFlow == OdfFlow.Flow2_3 || tmp.PFlow == OdfFlow.Flow8_1 || tmp.PFlow == OdfFlow.Flow8_2)
            {
                tmp.PBackToLD = true;
            }
            else
            {
                tmp.PBackToLD = false;
            }

            if(tmp.PFlow == OdfFlow.Flow6_1 || tmp.PFlow == OdfFlow.Flow6_2 || tmp.PFlow == OdfFlow.Flow6_3 || tmp.PFlow == OdfFlow.Flow7_2 ||
                tmp.PFlow == OdfFlow.Flow7_3 || tmp.PFlow == OdfFlow.Flow7_4)
            {
                tmp.PReworkFlow = true;
            }
            else
            {
                tmp.PReworkFlow = false;
            }


            tmp.PWaferVASDegree = float.Parse(cv_TxWaferOCR.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);
            tmp.PWaferIJPDegree = float.Parse(cv_TxWaferVas.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);
            tmp.PGlassVASDegree = float.Parse(cv_TxGlassVas.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);
            tmp.PTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            tmp.PDecription = lbl_RecipeDescription.Text.Trim();
            tmp.PSampling = int.Parse(cb_samplingRate.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);

            obj.Recipes.Add(tmp);
            Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDRecipeAction).Name, obj);
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void cb_Flow_SelectedIndexChanged(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.OdfFlow tmp = (CommonData.HIRATA.OdfFlow)Enum.Parse(typeof(CommonData.HIRATA.OdfFlow), cb_Flow.Text.Trim());
            if (cv_FlowDescription.ContainsKey(tmp))
            {
                txt_FlowDescription.Text = cv_FlowDescription[tmp];
            }
            else
            {
                txt_FlowDescription.Text = string.Empty;
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void btn_CurRecipe_Click(object sender, EventArgs e)
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string recipe = cb_CurRecipe.Text.Trim();
            int _irecipe = 0;
            if (int.TryParse(recipe, out _irecipe))
            {
                if (UiForm.cv_Recipes.IsRecipeExist(_irecipe.ToString()))
                {
                    if (IsHasCstLDCM())
                    {
                        CommonStaticData.PopForm("Please clean line(unload all port)", true, false);
                        return;

                    }
                    CommonData.HIRATA.MDRecipeAction obj = new MDRecipeAction();
                    obj.PType = MmfEventClientEventType.etRequest;
                    obj.PAction = DataEidtAction.SetCur;
                    List<RecipeItem> recipes = new List<RecipeItem>();
                    RecipeItem recipe_item = null;
                    if (UiForm.cv_Recipes.GetRecipeItem(_irecipe.ToString(), out recipe_item))
                    {
                        recipes.Add(recipe_item);
                        string rtn;
                        object tmp = null;
                        uint ticket;
                        string log = "";
                        obj.Recipes = recipes;
                        if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDRecipeAction).Name, obj, out ticket, out rtn, out tmp, 3000, KParseObjToXmlPropertyType.Field))
                        {
                            log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDRecipeAction).Name;
                        }
                    }
                }
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public void UpdateCurRecipeCombobox()
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (UiForm.cv_Recipes != null)
            {
                cb_CurRecipe.Items.Clear();
                cb_CurRecipe.SelectedIndex = -1;
                foreach (RecipeItem recipe in UiForm.cv_Recipes.cv_RecipeList)
                {
                    cb_CurRecipe.Items.Add(recipe.PId);
                }
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void cv_TxRecipeId_TextChanged(object sender, EventArgs e)
        {
            TextBox tmp = sender as TextBox;
            string recipe_id = tmp.Text.Trim();
            RecipeItem recipe = null;
            if (UiForm.cv_Recipes.GetRecipeItem(recipe_id, out recipe))
            {
                if (recipe != null)
                {
                    if (recipe.PVasNeedGlass)
                    {
                        if (!cb_NeedGlass.Checked)
                        {
                            cb_NeedGlass.Checked = true;
                        }
                    }
                    else
                    {
                        if (cb_NeedGlass.Checked)
                        {
                            cb_NeedGlass.Checked = false;
                        }
                    }

                    if (recipe.PBackToLD)
                    {
                        if (!cb_backToLd.Checked)
                        {
                            cb_backToLd.Checked = true;
                        }
                    }
                    else
                    {
                        if (cb_backToLd.Checked)
                        {
                            cb_backToLd.Checked = false;
                        }
                    }
                    if (recipe.PWaferPutUp)
                    {
                        if (!cb_waferPutUp.Checked)
                        {
                            cb_waferPutUp.Checked = true;
                        }
                    }
                    else
                    {
                        if (cb_waferPutUp.Checked)
                        {
                            cb_waferPutUp.Checked = false;
                        }
                    }
                    if (recipe.PFlipToUv)
                    {
                        if (!cb_FlipToUv.Checked)
                        {
                            cb_FlipToUv.Checked = true;
                        }
                    }
                    else
                    {
                        if (cb_FlipToUv.Checked)
                        {
                            cb_FlipToUv.Checked = false;
                        }
                    }
                    if (recipe.PReworkFlow)
                    {
                        if (!cb_Rework.Checked)
                        {
                            cb_Rework.Checked = true;
                        }
                    }
                    else
                    {
                        if (cb_Rework.Checked)
                        {
                            cb_Rework.Checked = false;
                        }
                    }
                }
            }
            else
            {
                if (cb_NeedGlass.Checked)
                {
                    cb_NeedGlass.Checked = false;
                }
                if (cb_backToLd.Checked)
                {
                    cb_backToLd.Checked = false;
                }
                if (cb_waferPutUp.Checked)
                {
                    cb_waferPutUp.Checked = false;
                }
                if (cb_FlipToUv.Checked)
                {
                    cb_FlipToUv.Checked = false;
                }
                if (cb_Rework.Checked)
                {
                    cb_Rework.Checked = false;
                }
            }
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
        public void UpdateSamplingDataCombobox()
        {
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (UiForm.cv_SamplingData != null)
            {
                cb_samplingRate.Items.Clear();
                cb_samplingRate.SelectedIndex = -1;
                foreach (SamplingIem item in UiForm.cv_SamplingData.cv_SamplingList)
                {
                    cb_samplingRate.Items.Add(item.PNo);
                }
            }
            UiForm.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}
