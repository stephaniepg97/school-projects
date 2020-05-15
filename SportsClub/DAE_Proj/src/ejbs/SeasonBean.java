package ejbs;

import dtos.SeasonDTO;
import entities.Season;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Stateless;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.util.Date;

@Stateless(name = "SeasonEJB")
public class SeasonBean extends Bean<Season, SeasonDTO> {

    @EJB
    private SportClassBean sportClassBean;

    @Override
    public Object primaryKey(SeasonDTO dto) {
        return dto.getId();
    }

    @Override
    protected SeasonDTO map(Season entity, SeasonDTO dto) {
        dto.setId(entity.getId());
        dto.setStartInMilliseconds(entity.getStart().getTime());
        dto.setEndInMilliseconds( entity.getEnd().getTime());
        return dto;
    }

    @Override
    protected Season map(SeasonDTO dto, Season entity) {
        entity.setId(dto.getId());
        entity.setStart(new Date(dto.getStartInMilliseconds()));
        entity.setEnd(new Date(dto.getEndInMilliseconds()));
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Season.class, SeasonDTO.class);
    }
}
