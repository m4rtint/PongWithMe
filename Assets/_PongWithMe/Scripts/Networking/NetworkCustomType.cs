using ExitGames.Client.Photon;

namespace PongWithMe
{
    public static class NetworkCustomType
    {
        private const int VECTOR2_SIZE = 2 * 4;
        private const int INT_SIZE = 4;
        
        public static void Register()
        {
            PhotonPeer.RegisterType(typeof(BrickDTO), (byte) 'A', SerializeBrickDTO, DeserializeBrickDTO);
        }

        private static readonly byte[] memBrickDTO = new byte[VECTOR2_SIZE + INT_SIZE];
        private static short SerializeBrickDTO(StreamBuffer outStream, object customobject)
        {
            BrickDTO bo = (BrickDTO) customobject;
            lock (memBrickDTO)
            {
                byte[] bytes = memBrickDTO;
                int index = 0;
                Protocol.Serialize(bo.PositionX, bytes, ref index);
                Protocol.Serialize(bo.PositionY, bytes, ref index);
                Protocol.Serialize(bo.PlayerOwned, bytes, ref index);
                outStream.Write(bytes, 0, memBrickDTO.Length);
            }

            return (short) memBrickDTO.Length;
        }

        private static object DeserializeBrickDTO(StreamBuffer inStream, short length)
        {
            BrickDTO bo = new BrickDTO();
            lock (memBrickDTO)
            {
                inStream.Read(memBrickDTO, 0, memBrickDTO.Length);
                int index = 0;
                Protocol.Deserialize(out bo.PositionX, memBrickDTO, ref index);
                Protocol.Deserialize(out bo.PositionY, memBrickDTO, ref index);
                Protocol.Deserialize(out bo.PlayerOwned, memBrickDTO, ref index);
            }

            return bo;
        }
    }
}

