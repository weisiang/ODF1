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
                    if (cv_CurFlowIncludeAoi)
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
                        if (m_Eq == EqId.SDP1 || m_Eq == EqId.SDP2 || m_Eq == EqId.SDP3)
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
                                            rtn = true ;
                                        }
                                    }
                                }
                                else
                                {
                                    rtn = false;
                                }
                            }
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
            int max_step = 0;
            int aoi_step = 0;
            for(int i=1; i <= max_step;i++)
            {
                if(m_jobMap.ContainsKey(i))
                {
                    if(first_step == 0)
                    {
                        first_step = i;
                        break;
                    }
                    if(m_jobMap[i].PTarget == ActionTarget.Eq && m_jobMap[i].PTargetId == (int)EqId.AOI)
                    {
                        if(aoi_step == 0)
                        {
                            aoi_step = i;
                        }
                    }
                }
            }
            //if(m_jobMap[first_step].PTarget == ActionTarget.Eq)
            if( (first_step != 0) && (aoi_step != 0))
            {
                if(m_jobMap[first_step].PTarget == ActionTarget.Eq)
                {
                    GlassData aoi_glass = getEqUnloadGlassDataExceptVas((int)EqId.AOI);
                    if( (aoi_glass != null)  && (!aoi_glass.IsNull()))
                    {
                        EqId spec_seal = getAoiSpecifySeal(aoi_glass);
                        if(spec_seal == (EqId)m_jobMap[first_step].PTargetId)
                        {
                            rtn = false;
                        }
                    }
                }
            }
            return false;
        }
        private bool FindJumpStep(EqId eq_id, Dictionary<int,RobotJob> m_JobMap , int now_step , out int jumpStep)
        {
            bool rtn = false;
            jumpStep = 0;

            if (cv_CurFlowIncludeAoi)
            {
                //1.aoi to seal.  2.aoi to next
                if (eq_id == EqId.AOI)
                {
                    GlassData aoi_glass = getEqUnloadGlassDataExceptVas((int)EqId.AOI);
                    if ((aoi_glass != null) && (!aoi_glass.IsNull()))
                    {
                        if (!IsAoiUnloadGlassHasAbnormalBit(aoi_glass))
                        {
                            int aoi_normal_next_step = getAoiNextStep();
                            jumpStep = aoi_normal_next_step;
                            rtn = true;
                        }
                        else
                        {
                            int second_seal_step = getSecondSealStep();
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
                        }
                        else
                        {
                            //seal -> aoi -> seal ->aoi. So we just scan next step (seal).
                            jumpStep = now_step + 1;
                        }
                    }
                    else
                    { // no need enter AOI by sampling. 
                        int aoi_normal_next_step = getAoiNextStep();
                        jumpStep = aoi_normal_next_step;
                    }
                }
            }

            return rtn;
        }
    }
}
