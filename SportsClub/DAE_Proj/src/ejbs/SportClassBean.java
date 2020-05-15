package ejbs;

import dtos.SportClassDTO;
import entities.Attendance;
import entities.SportClass;
import exceptions.MyEntityCannotBeDeletedException;
import exceptions.MyEntityNotFoundException;
import exceptions.MyReflectionException;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import java.util.Collections;
import java.util.List;

@Stateless(name = "SportClassEJB")
public class SportClassBean extends AbstractProductBean<SportClass, SportClassDTO> {

    @EJB
    private RankBean rankBean;

    @EJB
    private TimetableBean timetableBean;

    @EJB
    private CoachBean coachBean;

    @EJB
    private SeasonBean seasonBean;

    @EJB
    private AttendanceBean attendanceBean;

    @Override
    protected SportClassDTO map(SportClass entity, SportClassDTO dto) {
        dto = super.map(entity, dto);
        dto.setSeason(seasonBean.toDTO(entity.getSeason()));
        dto.setRank(rankBean.toDTO(entity.getRank()));
        dto.setTimetable(timetableBean.toDTO(entity.getTimetable()));
        dto.setCoach(coachBean.toDTO(entity.getCoach()));
        return dto;
    }

    @Override
    protected SportClass map(SportClassDTO dto, SportClass entity) {
        entity = super.map(dto, entity);
        entity.setSeason(seasonBean.toEntity(dto.getSeason()));
        entity.setRank(rankBean.toEntity(dto.getRank()));
        entity.setTimetable(timetableBean.toEntity(dto.getTimetable()));
        entity.setCoach(coachBean.toEntity(dto.getCoach()));
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(SportClass.class, SportClassDTO.class);
    }

    public List<Attendance> attendances(Integer id) {
        try {
            @SuppressWarnings("unchecked")
            List<Attendance> attendances = (List<Attendance>) em.createNamedQuery( "sportClass.attendances").setParameter("id", id).getResultList();
            attendances.removeAll(Collections.singletonList(null));
            return attendances;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_ATTENDANCES", e);
        }
    }

    @Override
    public void delete(Object primaryKey) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        for (Attendance a : attendances(Integer.valueOf(primaryKey.toString()))) attendanceBean.delete(a.getId());
        super.delete(primaryKey);
    }
}
