using Intel.RealSense;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public class DeviceInfoProvider
    {
        private readonly Pipeline pipeline;

        public DeviceInfoProvider(Pipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public string GetDeviceInfo()
        {
            var device = pipeline.ActiveProfile.Device;
            return $"Device Name: {device.Info[CameraInfo.Name]}, " +
                   $"Serial Number: {device.Info[CameraInfo.SerialNumber]}, " +
                   $"Firmware Version: {device.Info[CameraInfo.FirmwareVersion]}";
        }
    }
}
