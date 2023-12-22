// NetworkOptimizationHelper gotten from the TABG Client, on 21/12/2023, using ILspy
// If landfall ever has an issue with this, I will take it down!
// Guessing this can be removed once we have real car stuff since the clients send it I think

using System.Numerics;

namespace Landfall.Network
{
    public static class NetworkOptimizationHelper
    {
        private static readonly float QUATERNION_PRECISION_MULT = 10000f;
        private static readonly float DIRECTION_PRECISION_MULT = 100f;

        public static byte[] OptimizeQuaternion(Quaternion val)
        {
            byte b = 0;
            float num = Math.Abs(val.X);
            float num2 = 1f;
            if (Math.Abs(val.Y) > num)
            {
                b = 1;
                num = val.Y;
                num2 = ((val.Y < 0f) ? (-1f) : 1f);
            }
            if (Math.Abs(val.Z) > num)
            {
                b = 2;
                num = val.Z;
                num2 = ((val.Z < 0f) ? (-1f) : 1f);
            }
            if (Math.Abs(val.W) > num)
            {
                b = 3;
                num = val.W;
                num2 = ((val.W < 0f) ? (-1f) : 1f);
            }

            // https://www.reddit.com/r/Unity3D/comments/4iv728/req_can_someone_explain_mathfapproximately/
            if (Math.Abs(1f - num) < Math.Max(1E-06f * Math.Max(Math.Abs(num), Math.Abs(1f)), double.Epsilon * 8f)) 
            {
                b += 4;
                return new byte[1] { b };
            }
            short value;
            short value2;
            short value3;
            switch (b)
            {
                case 0:
                    value = (short)(val.Y * num2 * QUATERNION_PRECISION_MULT);
                    value2 = (short)(val.Z * num2 * QUATERNION_PRECISION_MULT);
                    value3 = (short)(val.W * num2 * QUATERNION_PRECISION_MULT);
                    break;
                case 1:
                    value = (short)(val.X * num2 * QUATERNION_PRECISION_MULT);
                    value2 = (short)(val.Z * num2 * QUATERNION_PRECISION_MULT);
                    value3 = (short)(val.W * num2 * QUATERNION_PRECISION_MULT);
                    break;
                case 2:
                    value = (short)(val.X * num2 * QUATERNION_PRECISION_MULT);
                    value2 = (short)(val.Y * num2 * QUATERNION_PRECISION_MULT);
                    value3 = (short)(val.W * num2 * QUATERNION_PRECISION_MULT);
                    break;
                default:
                    value = (short)(val.X * num2 * QUATERNION_PRECISION_MULT);
                    value2 = (short)(val.Y * num2 * QUATERNION_PRECISION_MULT);
                    value3 = (short)(val.Z * num2 * QUATERNION_PRECISION_MULT);
                    break;
            }
            byte[] array = new byte[7];
            using (MemoryStream output = new MemoryStream(array))
            using (BinaryWriter binaryWriter = new BinaryWriter(output))
            {
                binaryWriter.Write(b);
                binaryWriter.Write(value);
                binaryWriter.Write(value2);
                binaryWriter.Write(value3);
                return array;
            }
        }
        public static Quaternion ConstructQuaternion(byte maxIndex, byte[] data)
        {
            using MemoryStream input = new MemoryStream(data);
            using BinaryReader binaryReader = new BinaryReader(input);
            float num = (float)binaryReader.ReadInt16() / QUATERNION_PRECISION_MULT;
            float num2 = (float)binaryReader.ReadInt16() / QUATERNION_PRECISION_MULT;
            float num3 = (float)binaryReader.ReadInt16() / QUATERNION_PRECISION_MULT;
            float num4 = (float)Math.Sqrt(1f - (num * num + num2 * num2 + num3 * num3));
            return maxIndex switch
            {
                0 => new Quaternion(num4, num, num2, num3),
                1 => new Quaternion(num, num4, num2, num3),
                2 => new Quaternion(num, num2, num4, num3),
                _ => new Quaternion(num, num2, num3, num4),
            };
        }
        public static Quaternion ConstructQuaternion(byte data)
        {
            float x = ((data == 4) ? 1f : 0f);
            float y = ((data == 5) ? 1f : 0f);
            float z = ((data == 6) ? 1f : 0f);
            float w = ((data == 7) ? 1f : 0f);
            return new Quaternion(x, y, z, w);
        }
        public static byte[] OptimizeDirection(Vector3 dir)
        {
            byte b = (byte)(dir.X * DIRECTION_PRECISION_MULT + DIRECTION_PRECISION_MULT);
            byte b2 = (byte)(dir.Y * DIRECTION_PRECISION_MULT + DIRECTION_PRECISION_MULT);
            byte b3 = (byte)(dir.Z * DIRECTION_PRECISION_MULT + DIRECTION_PRECISION_MULT);
            return new byte[3] { b, b2, b3 };
        }
        public static Vector3 ConstructDirection(byte[] data)
        {
            float x = ((float)(int)data[0] - DIRECTION_PRECISION_MULT) / DIRECTION_PRECISION_MULT;
            float y = ((float)(int)data[1] - DIRECTION_PRECISION_MULT) / DIRECTION_PRECISION_MULT;
            float z = ((float)(int)data[2] - DIRECTION_PRECISION_MULT) / DIRECTION_PRECISION_MULT;
            return new Vector3(x, y, z);
        }
    }
}