#include "roborio.hpp"

namespace hel{
    RoboRIO::RoboRIO()noexcept{
        user_button = true;
        accelerometer = {};
        for(auto& a: accumulators){
            a = {};
        }
        alarm = {};
        analog_inputs = {};
        analog_outputs = {};
        can_motor_controllers = {};
        for(auto& a: counters){
            a = {};
        }
        digital_system = {};
        ds_errors = {};
        match_info = {};
        for(auto& a: encoder_managers){
            a = {};
        }
        for(auto& a: fpga_encoders){
            a = {};
        }
        global = {};
        for(auto& a: joysticks){
            a = {};
        }
        net_comm = {};
        power = {};
        pwm_system = {};
        relay_system = {};
        robot_mode = {};
        spi_system = {};
        watchdog = {};
    }

    RoboRIO::RoboRIO(const RoboRIO& source)noexcept{
#define COPY(NAME) NAME = source.NAME
        COPY(user_button);
        COPY(accelerometer);
        COPY(accumulators);
        COPY(alarm);
        COPY(analog_inputs);
        COPY(analog_outputs);
        COPY(can_motor_controllers);
        COPY(counters);
        COPY(digital_system);
        COPY(ds_errors);
        COPY(match_info);
        COPY(encoder_managers);
        COPY(fpga_encoders);
        COPY(global);
        COPY(joysticks);
        COPY(net_comm);
        COPY(power);
        COPY(pwm_system);
        COPY(relay_system);
        COPY(robot_mode);
        COPY(spi_system);
        COPY(watchdog);
#undef COPY
    }
    RoboRIO& RoboRIO::operator=(const RoboRIO& source){
        if(this != &source){
#define COPY(NAME) NAME = source.NAME
            COPY(user_button);
            COPY(accelerometer);
            COPY(accumulators);
            COPY(alarm);
            COPY(analog_inputs);
            COPY(analog_outputs);
            COPY(can_motor_controllers);
            COPY(counters);
            COPY(digital_system);
            COPY(ds_errors);
            COPY(match_info);
            COPY(encoder_managers);
            COPY(fpga_encoders);
            COPY(global);
            COPY(joysticks);
            COPY(net_comm);
            COPY(power);
            COPY(pwm_system);
            COPY(relay_system);
            COPY(robot_mode);
            COPY(spi_system);
            COPY(watchdog);
#undef COPY
        }
        return *this;
    }
}
