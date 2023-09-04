
using System;
using FFTWSharp;
using System.Runtime.InteropServices;

namespace MBalancingPC
{

	public class FFT_Class
	{
		IntPtr pin, pout;
		public float[] spectrumOut;
		public float[] spectrumPhase;
		GCHandle hin, hout;
		IntPtr fplan1;
			
		public FFT_Class()
		{
		}
		
		
		
		/// <summary>
		/// Do FFT fransform
		/// </summary>
	    /// <param name="length"></param> number of input samples
		/// <param name="data"></param> data to process
		/// <param name="useWindow"></param> Use window for data
		/// <returns></returns>
		public void ProcessFFT(int length, ref float[] data, bool useWindow)
		{
			int i;
            int n;

            if (length < 2)
            {
                n = 0;
                return;
            }
			n = length;
			// create two unmanaged arrays, properly aligned
			pin = fftwf.malloc(n * 8);
        	pout = fftwf.malloc(n * 8);
        	
        	// create two managed arrays, possibly misalinged
        	float[] fin = new float[n*2];
            float[] fout = new float[n*2];
            
            // get handles and pin arrays so the GC doesn't move them
            hin = GCHandle.Alloc(fin, GCHandleType.Pinned);
            hout = GCHandle.Alloc(fout, GCHandleType.Pinned);
              
			//prepare
			for (i = 0; i < n; i++)
			{
				fin[i*2] = data[i];
				fin[i*2 + 1] = 0;
				
				if (useWindow)
				{
					fin[i*2] = fin[i*2] * 
                        Convert.ToSingle(0.5*(1 - Math.Cos( 2*Math.PI*(i)/Convert.ToDouble(n-1)) ));
				}
			}
			
			Marshal.Copy(fin, 0, pin, n*2);
			fplan1 = fftwf.dft_1d(n, pin, pout, fftw_direction.Forward, fftw_flags.Estimate);
			
			fftwf.execute(fplan1);
			Marshal.Copy(pout, fout, 0, n);
			fftwf.free(pin);
        	fftwf.free(pout);
        	fftwf.destroy_plan(fplan1);
        	fftwf.cleanup();
        	
        	hin.Free();
        	hout.Free();

            spectrumOut = new float[n / 2];
            spectrumPhase = new float[n / 2];

            for (i = 0; i < (n-2); i+=2)
            {
                spectrumPhase[i / 2] = (float)Math.Atan2(Convert.ToDouble(fout[i + 1]), Convert.ToDouble(fout[i]));

                float val0 = Convert.ToSingle(fout[i]*fout[i] + fout[i+1]*fout[i+1]);
        		val0 = (float)Math.Sqrt(val0);
        		spectrumOut[i/2] = val0;
            }
		}


	}//end of class
}
