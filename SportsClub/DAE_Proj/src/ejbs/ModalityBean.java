package ejbs;

import dtos.ModalityDTO;
import dtos.ProductDTO;
import entities.Graduation;
import entities.Modality;
import entities.Product;
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
import java.util.Set;

@Stateless(name = "ModalityEJB")
public class ModalityBean extends Bean<Modality, ModalityDTO> {

    @EJB
    private ProductBean productBean;

    @EJB
    private GraduationBean graduationBean;

    @EJB
    private SportClassBean sportClassBean;

    @Override
    public Object primaryKey(ModalityDTO dto) {
        return dto.getId();
    }

    @Override
    protected ModalityDTO map(Modality entity, ModalityDTO dto) {
        dto.setId(entity.getId());
        dto.setName(entity.getName());
        return dto;
    }

    @Override
    protected Modality map(ModalityDTO dto, Modality entity) {
        entity.setId(dto.getId());
        entity.setName(dto.getName());
        return entity;
    }

    @Override
    @PostConstruct
    protected void init() {
        initialize(Modality.class, ModalityDTO.class);
    }

    public List<Graduation> graduations(Integer id) {
        try {
            @SuppressWarnings("unchecked")
            List<Graduation> graduations = (List<Graduation>) em.createNamedQuery( "modality.graduations").setParameter("id", id).getResultList();
            graduations.removeAll(Collections.singletonList(null));
            return graduations;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_GRADUATIONS", e);
        }
    }

    public List<SportClass> classes(Integer id) {
        try {
            @SuppressWarnings("unchecked")
            List<SportClass> classes = (List<SportClass>) em.createNamedQuery( "modality.classes").setParameter("id", id).getResultList();
            classes.removeAll(Collections.singletonList(null));
            return classes;
        } catch (Exception e) {
            throw new EJBException("ERROR_RETRIEVING_GRADUATIONS", e);
        }
    }

    @Override
    public void delete(Object primaryKey) throws MyEntityNotFoundException, MyReflectionException, MyEntityCannotBeDeletedException {
        for (Graduation g : graduations(Integer.valueOf(primaryKey.toString()))) graduationBean.delete(g.getId());
        for (SportClass s : classes(Integer.valueOf(primaryKey.toString()))) sportClassBean.delete(s.getId());
        super.delete(primaryKey);
    }
}

   /* public void enrollModalityInAthelete(int idModality, String username) throws MyEntityNotFoundException {
        Modality modality = findModality(idModality);
        Athelete athelete = atheleteBean.findAthelete(username);
        if (modality == null || athelete == null)
            return;
        athelete.addModality(modality);
        modality.addAthelete(athelete);
        em.merge(modality);
    }

    public void unrollModalityFromAthelete(int idModality, String username) {
        Athelete athelete = em.find(Athelete.class, username);
        Modality modality = em.find(Modality.class, idModality);

        if (athelete == null || modality == null) {
            return;
        }

        if (!modality.getRanks().contains(athelete)) {
            return;
        }

        modality.removeAthelete(athelete);

        athelete.removeModality(modality);

        em.merge(modality);
        em.merge(athelete);

    }*/
