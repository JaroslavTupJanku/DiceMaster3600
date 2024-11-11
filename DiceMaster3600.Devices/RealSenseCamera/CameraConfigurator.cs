using Intel.RealSense;
using System;
using System.Linq;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public class CameraConfigurator
    {
        private readonly Pipeline pipeline;

        public CameraConfigurator(Pipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void ConfigureStream(Stream streamType, int width, int height, Format format, int fps)
        {
            var config = new Config();
            config.EnableStream(streamType, width, height, format, fps);
            pipeline.Start(config);
        }

        public void SetOption(Stream streamType, Option option, float value)
        {
            var sensorList = pipeline.ActiveProfile.Device.QuerySensors();
            foreach (var sensor in sensorList)
            {
                if (sensor.StreamProfiles.Any(sp => sp.Stream == streamType))
                {
                    sensor.Options[option].Value = value;
                }
            }
        }

        public float GetOption(Stream streamType, Option option)
        {
            var sensorList = pipeline.ActiveProfile.Device.QuerySensors();

            foreach (var sensor in sensorList)
            {
                if (sensor.StreamProfiles.Any(sp => sp.Stream == streamType))
                {
                    return sensor.Options[option].Value;
                }
            }

            throw new InvalidOperationException("Option not found for the specified stream type.");
        }
    }
}
