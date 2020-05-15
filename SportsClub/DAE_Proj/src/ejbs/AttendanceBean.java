package ejbs;

import dtos.AttendanceDTO;
import entities.Attendance;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Stateless;
import java.util.Date;

@Stateless(name = "AttendanceEJB")
public class AttendanceBean extends Bean<Attendance, AttendanceDTO> {

    @EJB
    private AthleteBean athleteBean;

    @EJB
    private SportClassBean sportClassBean;

    @Override
    public Object primaryKey(AttendanceDTO dto) {
        return dto.getId();
    }

    @Override
    protected AttendanceDTO map(Attendance entity, AttendanceDTO dto) {
        dto.setId(entity.getId());
        dto.setAthlete(athleteBean.toDTO(entity.getAthlete()));
        dto.setSportClass(sportClassBean.toDTO(entity.getSportClass()));
        dto.setDateInMilliseconds(entity.getDate().getTime());
        return dto;
    }

    @Override
    protected Attendance map(AttendanceDTO dto, Attendance entity) {
        entity.setId(dto.getId());
        entity.setAthlete(athleteBean.toEntity(dto.getAthlete()));
        entity.setSportClass(sportClassBean.toEntity(dto.getSportClass()));
        entity.setDate(new Date(dto.getDateInMilliseconds()));
        return entity;
    }
    @Override
    @PostConstruct
    protected void init() {
        initialize(Attendance.class, AttendanceDTO.class);
    }
}
