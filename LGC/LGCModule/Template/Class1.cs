using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;
using CommonData;
using BaseAp;
using CommonData.HIRATA;

namespace LGC
{
    public partial class LgcForm
    {
        private bool checkPathConditionAtEqLoad(EqId m_Eq , Dictionary<int,RobotJob> m_JobMap , int now_step)// , GlassData m_GlassData)
        {
            bool rtn = false;
            RecipeItem recipe = null;
            if (cv_Recipes.GetCurRecipe(out recipe))
            {
                if (m_Eq == EqId.AOI)
                {
                    if (cv_CurFlowIncludeAoi && (!recipe.PReworkFlow))
                    {
                        if (!checkpathHasAoiInMiddle(m_JobMap, now_step))
                        {
                            rtn = true;
                        }
                    }
                }
                else if (m_Eq == EqId.SDP1 ||m_Eq == EqId.SDP2 ||m_Eq == EqId.SDP3 )
                {
                    if (recipe.PReworkFlow)
                    {
                        int aoi_step = FindAoiStepInJobpath(m_JobMap, now_step);
                        if (aoi_step != 0)
                        {
                            GlassData aoi_glass = getEqUnloadGlassDataExceptVas((int)EqId.AOI);
                            if ((aoi_glass != null) && (!aoi_glass.IsNull()))
                            {
                                if (IsAoiUnloadGlassHasAbnormalBit(aoi_glass))
                                {
                                    if (getAoiSpecifySeal(aoi_glass) == m_Eq)
                                    {
                                        rtn = true;
                                    }
                                }
                                else
                                {
                                    //this case is imposible in current flow setting. 
                                    rtn = true;
                                }
                            }
                            else
                            {
                                rtn = false;
                            }
                        }
                        else
                        {
                            //job path don't include AOI , so don't care which Seal can load.
                            rtn = true;
                        }
                    }
                }
            }
            return rtn;
        }
        private bool CanGetNewSubstratePutToSeal(Queue<RobotJob> m_jobMap)
        {
            bool rtn = true;
            //int first_step = 0;
            //int max_step = 0;
            int aoi_step = 0;
            for (int i = 0; i < m_jobMap.Count; i++)
            {
                if (m_jobMap.ElementAt(i).PTarget == ActionTarget.Eq && m_jobMap.ElementAt(i).PTargetId == (int)EqId.AOI)
                {
                    if (aoi_step == 0)
                    {
                        aoi_step = i;
                    }
                }
            }
            //if(m_jobMap[first_step].PTarget == ActionTarget.Eq)
            if ((aoi_step != 0))
            {
                if (m_jobMap.Peek().PTarget == ActionTarget.Eq)
                {
                    GlassData aoi_glass = getEqUnloadGlassDataExceptVas((int)EqId.AOI);
                    if ((aoi_glass != null) && (!aoi_glass.IsNull()))
                    {
                        EqId spec_seal = getAoiSpecifySeal(aoi_glass);
                        if (spec_seal == (EqId)m_jobMap.Peek().PTargetId)
                        {
                            rtn = false;
                        }
                    }
                }
            }
            return false;
        }
        private bool CanGetNewSubstratePutToSeal(Dictionary<int , RobotJob> m_jobMap)
        {
            bool rtn = true;
            int first_step = 0;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int aoi_step = 0;
            int last_step = 0;
            RecipeItem recipe = null;
            if(!cv_Recipes.GetCurRecipe(out recipe))
            {
                rtn = false;
                return false;
            }
            if (recipe.PReworkFlow)
            {
                for (int i = 1; i <= max_step; i++)
                {
                    if (m_jobMap.ContainsKey(i))
                    {
                        last_step = i;
                        if (first_step == 0)
                        {
                            first_step = i;
                        }
                        if (m_jobMap[i].PTarget == ActionTarget.Eq && m_jobMap[i].PTargetId == (int)EqId.AOI)
                        {
                            if (aoi_step == 0)
                            {
                                aoi_step = i;
                            }
                        }
                    }
                }
                //if(m_jobMap[first_step].PTarget == ActionTarget.Eq)
                if ((first_step != 0) && (aoi_step != 0))
                {
                    if (m_jobMap[first_step].PTarget == ActionTarget.Eq)
                    {
                        if (last_step != aoi_step)
                        {
                            GlassData aoi_glass = getEqUnloadGlassDataExceptVas((int)EqId.AOI);
                            if ((aoi_glass != null) && (!aoi_glass.IsNull()))
                            {
                                //need add checking aoi abnormal bit.
                                bool abnormal_aoi = IsAoiUnloadGlassHasAbnormalBit(aoi_glass);
                                if (abnormal_aoi)
                                {
                                    EqId spec_seal = getAoiSpecifySeal(aoi_glass);
                                    if (spec_seal == (EqId)m_jobMap[first_step].PTargetId)
                                    {
                                        rtn = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rtn;
        }
        private bool JobpathalreayhasSealAtHead(Dictionary<int,RobotJob> m_jobMap , out EqId sealId)
        {
            bool rtn = false;
            sealId = EqId.None;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int headIndex = -1;
            for (int i = 1; i <= max_step; i++)
            {
                if (m_jobMap.ContainsKey(i))
                {
                    if (headIndex == -1)
                    {
                        headIndex++;
                        if (m_jobMap[i].PTarget == ActionTarget.Eq && 
                            (m_jobMap[i].PTargetId == (int)EqId.SDP1) || 
                            (m_jobMap[i].PTargetId == (int)EqId.SDP2) || 
                            (m_jobMap[i].PTargetId == (int)EqId.SDP3) )
                        {
                            sealId = (EqId)m_jobMap[i].PTargetId;
                            rtn = true;
                            break;
                        }
                    }
                }
            }
            return rtn;
        }
        private bool JobpathalreayhasAoi(Dictionary<int,RobotJob> m_jobMap)
        {
            bool rtn = false;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            for (int i = 1; i <= max_step; i++)
            {
                if (m_jobMap.ContainsKey(i))
                {
                    if (m_jobMap[i].PTarget == ActionTarget.Eq && m_jobMap[i].PTargetId == (int)EqId.AOI)
                    {
                        rtn = true;
                        break;
                    }
                }
            }
            return rtn;
        }
        private bool FindJumpStep(EqId eq_id, Dictionary<int,RobotJob> m_JobMap , int now_step , out int jumpStep)
        {
            bool rtn = false;
            RecipeItem recipe = null;
            jumpStep = 0;
            if(!cv_Recipes.GetCurRecipe(out recipe))
            {
                return rtn;
            }

            if (cv_CurFlowIncludeAoi)
            {
                //1.aoi to seal.  2.aoi to next
                if (eq_id == EqId.AOI)
                {
                    GlassData aoi_glass = getEqUnloadGlassDataExceptVas((int)EqId.AOI);
                    if ((aoi_glass != null) && (!aoi_glass.IsNull()))
                    {
                        if ( (!IsAoiUnloadGlassHasAbnormalBit(aoi_glass) || (!recipe.PReworkFlow)))
                        {
                            int aoi_normal_next_step = getAoiNextStep();
                            jumpStep = aoi_normal_next_step;
                            rtn = true;
                        }
                        else
                        {
                            int second_seal_step = getSecondSealStep();
                            //special case : flow setting : seal -> aoi -> seal -> aoi
                            // if we meet : first aoi no signal , then when we do second aoi has signal.
                            // Prevent we do step number checking. then return false. in FindJumpStep.
                            if(second_seal_step > now_step)
                            {
                                jumpStep = second_seal_step;
                                rtn = true;
                            }
                        }
                    }
                }
                else if (eq_id == EqId.SDP1 || eq_id == EqId.SDP2 || eq_id == EqId.SDP3)
                { //1.seal -> aoi , 2.seal to ijp. 3.seal to aoi next.
                    GlassData seal_glass = getEqUnloadGlassDataExceptVas((int)eq_id);
                    if (GlassNeedEnterAoi(seal_glass))//check sampling.
                    {
                        //rework glass , go to ijp directly.
                        if (AlradyEnterAoi(seal_glass))
                        {
                            int ijp_step = getIjpStep();
                            jumpStep = ijp_step;
                            rtn = true;
                        }
                        else
                        {
                            //seal -> aoi -> seal ->aoi. So we just scan next step (seal).
                            jumpStep = now_step + 1;
                            rtn = true;
                        }
                    }
                    else
                    { // no need enter AOI by sampling. 
                        int aoi_normal_next_step = getAoiNextStep();
                        jumpStep = aoi_normal_next_step;
                        rtn = true;
                    }
                }
            }
            return rtn;
        }
        private int getSecondSealStep()
        {
            int first_seal_step = 0;
            int second_seal_step = 0;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int first_step = 1;
            for (int step = first_step; step < max_step; step++)
            {
                if (cv_CurRecipeFlowStepSetting.ContainsKey(step))
                {
                    if (cv_CurRecipeFlowStepSetting[step].Contains(AllDevice.SDP1) ||
                        cv_CurRecipeFlowStepSetting[step].Contains(AllDevice.SDP2) ||
                        cv_CurRecipeFlowStepSetting[step].Contains(AllDevice.SDP3))
                    {
                        if (first_seal_step == 0)
                        {
                            first_seal_step = step;
                        }
                        else if (second_seal_step == 0)
                        {
                            second_seal_step = step;
                            break;
                        }
                    }
                }
            }
            return second_seal_step;
        }
        private bool IsAoiUnloadGlassHasAbnormalBit(GlassData m_AoiGlass)
        {
            //aoi is node 9.
            bool rtn = false;
            Eq eq = GetEqById((int)EqId.AOI);
            if ((m_AoiGlass != null) && (!m_AoiGlass.IsNull()))
            {
                int node_index = m_AoiGlass.cv_Nods.FindIndex(x => x.PNodeId == eq.PNode);
                if (m_AoiGlass.cv_Nods[node_index].cv_ProcessAbnormal != 0)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        private EqId getAoiSpecifySeal(GlassData m_AoiGlass)
        {
            //seal1 : node 3 , seal2 : node4 , seal3 : node8.
            EqId rtn = EqId.None;
            if ((m_AoiGlass != null) && (!m_AoiGlass.IsNull()))
            {
                int node_index = m_AoiGlass.cv_Nods.FindIndex(x => x.PNodeId == 3);
                if (m_AoiGlass.cv_Nods[node_index].cv_ProcessHistory != 0)
                {
                    rtn = EqId.SDP1;
                }
                node_index = m_AoiGlass.cv_Nods.FindIndex(x => x.PNodeId == 4);
                if (m_AoiGlass.cv_Nods[node_index].cv_ProcessHistory != 0)
                {
                    rtn = EqId.SDP2;
                }
                /*
                node_index = m_AoiGlass.cv_Nods.FindIndex(x => x.PNodeId == 8);
                if (m_AoiGlass.cv_Nods[node_index].cv_ProcessHistory != 0)
                {
                    rtn = EqId.SDP3;
                }
                */
            }
            return rtn;
        }
        private GlassData getEqUnloadGlassDataExceptVas(int eqid)
        {
            GlassData rtn = null;
            TimechartNormal time_chart_instance = null;
            int time_chart_id = -1;
            time_chart_id = GetEqById(eqid).cv_Comm.cv_TimeChatId;
            time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
            rtn = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
            return rtn;
        }
        private int FindAoiStepInJobpath(Dictionary<int, RobotJob> m_jobpath, int m_NowStep)
        {
            int rtn = 0;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int first_step = 1;
            for (int step = first_step; step < m_NowStep; step++)
            {
                if (m_jobpath.ContainsKey(step))
                {
                    if (m_jobpath[step].PTarget == ActionTarget.Eq && m_jobpath[step].PTargetId == (int)EqId.AOI)
                    {
                        rtn = step;
                    }
                }
            }
            return rtn;
        }
        private bool checkpathHasAoiInMiddle(Dictionary<int, RobotJob> m_jobpath, int m_NowStep)
        {
            bool rtn = false;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int first_step = 1;
            int acture_first_step = 0;
            for (int step = first_step; step < m_NowStep; step++)
            {
                if (m_jobpath.ContainsKey(step))
                {
                    if (acture_first_step == 0)
                    {
                        acture_first_step = step;
                    }
                    if (m_jobpath[step].PTarget == ActionTarget.Eq && m_jobpath[step].PTargetId == (int)EqId.AOI)
                    {
                        if (step != acture_first_step)
                        {
                            rtn = true;
                            break;
                        }
                    }
                }
            }
            return rtn;
        }
        private int getAoiNextStep()
        {
            //cv_CurRecipeFlowStepSetting = new Dictionary<int, List<AllDevice>>();
            int rtn = 0;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int first_step = 1;
            for (int step = first_step; step <= max_step; step++)
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[step];
                if (cv_stepDevice.Contains(AllDevice.AOI))
                {
                    rtn = step + 1;
                }
            }
            return rtn;
        }
        public static bool alreadytakeout(GlassData m_Glass)
        {
            bool rtn = false;
            if (m_Glass != null)
            {
                int node_index = m_Glass.cv_Nods.FindIndex(x => x.PNodeId == 2);
                if ((m_Glass.cv_Nods[node_index].PProcessHistory & 0x1) == 1)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        private bool AlradyEnterAoi(GlassData m_Glass)
        {
            int aoi_node = GetEqById((int)EqId.AOI).PNode;
            bool rtn = false;
            if (m_Glass != null)
            {
                int node_index = m_Glass.cv_Nods.FindIndex(x => x.PNodeId == aoi_node);
                if (m_Glass.cv_Nods[node_index].PProcessHistory != 0)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        private bool GlassNeedEnterAoi(GlassData m_Glass)
        {
            bool rtn = false;
            if (m_Glass != null)
            {
                int node_index = m_Glass.cv_Nods.FindIndex(x => x.PNodeId == 2);
                int need = (m_Glass.cv_Nods[node_index].PProcessHistory & 0x2) >> 1;
                if (need == 1)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        private int getIjpStep()
        {
            int rtn = 0;
            int max_step = cv_CurRecipeFlowStepSetting.Count;
            int first_step = 1;
            for (int step = first_step; step <= max_step; step++)
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[step];
                if (cv_stepDevice.Contains(AllDevice.IJP))
                {
                    rtn = step;
                }
            }
            return rtn;
        }
    }
}
