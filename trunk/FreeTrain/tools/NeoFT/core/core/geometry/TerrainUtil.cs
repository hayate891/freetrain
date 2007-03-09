using System;

namespace nft.core.geometry
{
	/// <summary>
	/// TerrainUtil �̊T�v�̐����ł��B
	/// </summary>
	public class TerrainUtil
	{
		static readonly short[] vtx_step = new short[]{0,1,2,2,4,4,4,4,8};
		static int[] workarray = new int[4];
		static int[] idx = new int[4];

		/// <summary>
		/// �{�N�Z���̒��_���K��p�^�[���ɍ����悤�ɕ␳����		/// 
		/// </summary>
		/// <param name="vertices"></param>
		/// <returns>���_�̍ŏ��l��Ԃ�</returns>
		static public int CorrectVoxelVertices(ref int[] vertices)
		{
			// MEMO:min�܂���max�ƈقȂ�l�́A�S��med���Ԓl�Ƃ���ׂ�
			vertices.CopyTo(workarray,0);
			for( int i=0; i<4; i++)
				idx[i] = i;
			Array.Sort(workarray,idx);
			int minimum = workarray[0];
			for( int i=0; i<4; i++){
				workarray[i]-=minimum;
				vertices[i]-=minimum;
			}

			// �S�Ă̖ʂ�������
			if(workarray[0]==workarray[3])
				return minimum;
			// �ő�l�ƍŏ��l�̍�
			int gap = workarray[3]-workarray[0];
			// �ő�l�ƍŏ��l�̍���0,1,2,4,8�̂����ꂩ�ɏk���␳
			if(gap>8) gap = 8;
			else gap = vtx_step[gap];

			int tmp;
			// �O�_���ŏ��l
			if(workarray[0]==workarray[2])
			{
				vertices[idx[3]]=workarray[0]+gap;
				return minimum;
			}
			// �O�_���ő�l
			if(workarray[1]==workarray[3])
			{
				vertices[idx[0]]=workarray[3]-gap;
				return minimum;
			}
			// ��_���ŏ��l
			if(workarray[0]==workarray[1])
			{
				// ��_���ŏ��l����_���ő�l
				if(workarray[2]==workarray[3])
				{
					// �����̒��_����ɂ���
					int org = (workarray[1]+workarray[2]-gap)>>1;
					vertices[idx[0]]=vertices[idx[1]]=org;
					vertices[idx[2]]=vertices[idx[3]]=org+gap;
					return minimum;
				}
				// ��_���ŏ��l����_�݂̂��ő�l
				else
				{
					// �ŏ��l����ɂ���	
					tmp = workarray[0]+gap;
					vertices[idx[3]] = tmp;
					if( tmp < workarray[2] )
						vertices[idx[2]] = tmp;
					else
						vertices[idx[2]]=workarray[0]+(gap>>1);
					return minimum;
				}
			}
			// ��_���ő�l����_�݂̂��ŏ��l
			if(workarray[2]==workarray[3])
			{
				// �ő�l����ɂ���
				tmp =workarray[3]-gap;
				vertices[idx[0]] = tmp;
				if( workarray[1] < tmp )
					vertices[idx[1]] = tmp;
				else
					vertices[idx[1]]=vertices[idx[0]]+(gap>>1);
				return minimum;
			}
			else 
			{
				// �ő�l�ƍŏ��l���e��_�Â�
				// �����̒��_����ɂ���
				int org = (workarray[0]+workarray[3]-gap)>>1;
				vertices[idx[0]]=org;
				vertices[idx[3]]=org+gap;
				tmp = org+(gap>>1);
				if(workarray[1]<org)
					vertices[idx[1]] = org;
				else
					vertices[idx[1]] = tmp;
				if(vertices[idx[3]]<workarray[2])
					vertices[idx[2]]=vertices[idx[3]];
				else
					vertices[idx[2]] = tmp;
				//return minimum;
			}
			return minimum;
		}
	}
}
