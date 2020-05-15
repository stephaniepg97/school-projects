package ejbs;

import dtos.RankDTO;
import entities.Rank;

import javax.annotation.PostConstruct;
import javax.ejb.EJB;
import javax.ejb.Stateless;

@Stateless(name = "RankEJB")
public class RankBean extends Bean<Rank, RankDTO> {

    @Override
    public Object primaryKey(RankDTO dto) {
        return dto.getName();
    }

    @Override
    protected RankDTO map(Rank entity, RankDTO dto) {
        dto.setName(entity.getName());
        return dto;
    }

    @Override
    protected Rank map(RankDTO dto, Rank entity) {
        entity.setName(dto.getName());
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Rank.class, RankDTO.class);
    }
}
