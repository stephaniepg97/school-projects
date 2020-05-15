package ejbs;

import dtos.TimetableDTO;
import entities.Timetable;

import javax.annotation.PostConstruct;
import javax.ejb.Stateless;
import java.time.DayOfWeek;
import java.time.LocalTime;

@Stateless(name = "TimetableEJB")
public class TimetableBean extends Bean<Timetable, TimetableDTO> {

    @Override
    public Object primaryKey(TimetableDTO dto) {
        return dto.getId();
    }

    @Override
    protected TimetableDTO map(Timetable entity, TimetableDTO dto) {
        dto.setId(entity.getId());
        dto.setDayOfWeek(entity.getDayOfWeek().getValue());
        dto.setSecondOfDayDuration(entity.getDuration().toSecondOfDay());
        dto.setSecondOfDayInit(entity.getInit().toSecondOfDay());
        return dto;
    }

    @Override
    protected Timetable map(TimetableDTO dto, Timetable entity) {
        entity.setId(dto.getId());
        entity.setDayOfWeek(DayOfWeek.of(dto.getDayOfWeek()));
        entity.setDuration(LocalTime.ofSecondOfDay(dto.getSecondOfDayDuration()));
        entity.setInit(LocalTime.ofSecondOfDay(dto.getSecondOfDayInit()));
        return entity;
    }
    @Override
    @PostConstruct
    protected void init() {
        initialize(Timetable.class, TimetableDTO.class);
    }
}
