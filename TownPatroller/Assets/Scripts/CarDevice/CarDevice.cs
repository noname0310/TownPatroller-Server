using System;

namespace TownPatroller.CarDevice
{
    [Serializable]
    public class Cardevice
    {
        public ushort f_sonardist { get { return F_sonardist; } }
        public ushort rh_sonardist { get { return RH_sonardist; } }
        public ushort lh_sonardist { get { return LH_sonardist; } }
        public ushort rs_sonardist { get { return RS_sonardist; } }
        public ushort ls_sonardist { get { return LS_sonardist; } }
        public byte r_motorpower
        {
            get
            {
                return R_motorpower;
            }
            set
            {
                Set_R_motorpower(value);
            }
        }
        public byte l_motorpower
        {
            get
            {
                return L_motorpower;
            }
            set
            {
                Set_L_motorpower(value);
            }
        }
        public bool r_motorDIR
        {
            get
            {
                return R_motorDIR;
            }
            set
            {
                Set_R_motorDIR(value);
            }
        }
        public bool l_motorDIR
        {
            get
            {
                return L_motorDIR;
            }
            set
            {
                Set_L_motorDIR(value);
            }
        }
        public bool rf_LED
        {
            get
            {
                return RF_LED;
            }
            set
            {
                Set_RF_LED(value);
            }
        }
        public bool lf_LED
        {
            get
            {
                return LF_LED;
            }
            set
            {
                Set_LF_LED(value);
            }
        }
        public bool rb_LED
        {
            get
            {
                return RB_LED;
            }
            set
            {
                Set_RB_LED(value);
            }
        }
        public bool lb_LED
        {
            get
            {
                return LB_LED;
            }
            set
            {
                Set_LB_LED(value);
            }
        }

        protected ushort F_sonardist;
        protected ushort RH_sonardist;
        protected ushort LH_sonardist;
        protected ushort RS_sonardist;
        protected ushort LS_sonardist;

        protected byte R_motorpower;
        protected byte L_motorpower;
        protected bool R_motorDIR;
        protected bool L_motorDIR;
        protected bool RF_LED;
        protected bool LF_LED;
        protected bool RB_LED;
        protected bool LB_LED;

        protected virtual void Set_R_motorpower(byte value)
        {
            R_motorpower = value;
        }

        protected virtual void Set_L_motorpower(byte value)
        {
            L_motorpower = value;
        }

        protected virtual void Set_R_motorDIR(bool value)
        {
            R_motorDIR = value;
        }

        protected virtual void Set_L_motorDIR(bool value)
        {
            L_motorDIR = value;
        }

        protected virtual void Set_RF_LED(bool value)
        {
            RF_LED = value;
        }

        protected virtual void Set_LF_LED(bool value)
        {
            LF_LED = value;
        }

        protected virtual void Set_RB_LED(bool value)
        {
            RB_LED = value;
        }

        protected virtual void Set_LB_LED(bool value)
        {
            LB_LED = value;
        }
    }
}