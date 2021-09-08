
using System.Collections.Generic;
using UnityEngine;

namespace PongWithMe
{
    public static class BrickToDTO 
    {
        public static BrickDTO[] ConvertToDTO(Brick[] bricks)
        {
            List<BrickDTO> listOfDTO = new List<BrickDTO>();
            foreach (var brick in bricks)
            {
                var brickDTO = new BrickDTO();
                brickDTO.PlayerOwned = brick.PlayerOwned;
                brickDTO.PositionX = brick.Position.x;
                brickDTO.PositionY = brick.Position.y;
                listOfDTO.Add(brickDTO);
            }

            return listOfDTO.ToArray();
        }

        public static Brick[] ConvertToBricks(BrickDTO[] bricks)
        {
           List<Brick> list = new List<Brick>();
           foreach (var brickDTO in bricks)
           {
               var brick = new Brick
               {
                   Position = new Vector3(brickDTO.PositionX, brickDTO.PositionY),
                   PlayerOwned = brickDTO.PlayerOwned,
                   BrickColor = ColorPalette.PlayerColor(brickDTO.PlayerOwned),
                   IsActive = true
               };
               list.Add(brick);
           }

           return list.ToArray();
        }
    }
}

