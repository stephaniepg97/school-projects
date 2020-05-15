package ejbs;

import dtos.DTO;
import entities.Entity;
import exceptions.*;

import javax.ejb.EJBException;
import javax.persistence.EntityManager;
import javax.persistence.LockModeType;
import javax.persistence.PersistenceContext;
import javax.validation.ConstraintViolationException;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public abstract class Bean<E extends Entity, D extends DTO>  {

    private Class<E> entityClass;
    private Class<D> dtoClass;

    @PersistenceContext
    protected EntityManager em;

    public D  create(D dto) throws MyConstraintViolationException, MyEntityExistsException {
        try {
            if (dto == null) return null;
            Object primaryKey = primaryKey(dto);
            if (primaryKey != null && findOrFail(primaryKey) != null) throw new MyEntityExistsException(entityClass, primaryKey);
            E entity = toEntity(dto);
            em.persist(entity);
            em.flush();
            return toDTO(entity);
        } catch (MyEntityExistsException e) {
            throw e;
        } catch (ConstraintViolationException e) {
            throw new MyConstraintViolationException(Utils.getConstraintViolationMessages(e));
        } catch (Exception e) {
            throw new EJBException(errorMsg("creating"), e);
        }
    }

    public D update(D dto) throws MyEntityNotFoundException, MyConstraintViolationException {
        try {
            if (dto == null) return null;
            E entity = find(primaryKey(dto));
            em.lock(entity, LockModeType.OPTIMISTIC);
            entity = toEntity(dto);
            em.persist(entity);
            em.flush();
            return toDTO(entity);
        } catch (MyEntityNotFoundException e) {
            throw e;
        } catch (ConstraintViolationException e) {
            throw new MyConstraintViolationException(Utils.getConstraintViolationMessages(e));
        } catch (Exception e) {
            throw new EJBException(errorMsg("updating"), e);
        }
    }

    public void delete(Object primaryKey) throws MyEntityNotFoundException, MyEntityCannotBeDeletedException, MyReflectionException {
        try {
            if (primaryKey == null) return;
            E entity = find(primaryKey);
            em.remove(entity);
        } catch (MyEntityNotFoundException e) {
            throw e;
        } catch (Exception e) {
            throw new EJBException(errorMsg("deleting"), e);
        }
    }

    public E toEntity(D dto) {
        try {
            if (entityClass == null) throw new MyReflectionException(null, "finding generic type or class: ENTITY");
            if (dto == null) return null;
            E entity = findOrFail(primaryKey(dto));
            if (entity == null) entity = entityClass.newInstance();
            return map(dto, entity);
        } catch (Exception e) {
            throw new EJBException(errorMsg("converting") + "TO_ENTITY", e);
        }
    }

    public D toDTO(E entity) {
        try {
            if (dtoClass == null) throw new MyReflectionException(null, "finding generic type or class: DTO");
            if (entity == null) return null;
            D dto = dtoClass.newInstance();
            return map(entity, dto);
        } catch (Exception e) {
            throw new EJBException(errorMsg("converting") + "TO_DTO", e);
        }
    }

    public Set<D> toDTOs(Set<E> entities) {
        return entities.stream().map(this::toDTO).collect(Collectors.toSet());
    }

    public Set<E> toEntities(Set<D> dtos) {
        return dtos.stream().map(this::toEntity).collect(Collectors.toSet());
    }

    public List<E> all() throws MyReflectionException {
        try {
            if (entityClass == null) throw new MyReflectionException(null, "finding generic type or class: ENTITY");
            char[] entityChars = entityClass.getSimpleName().toCharArray();
            entityChars[0] = Character.toLowerCase(entityChars[0]);
            @SuppressWarnings("unchecked")
            List<E> result = (List<E>) em.createNamedQuery( String.valueOf(entityChars) + ".all").getResultList();
            return result;
        } catch (MyReflectionException e) {
            throw e;
        } catch (Exception e) {
            throw new EJBException(errorMsg("retrieving") + "S", e);
        }
    }

    public E find(Object primaryKey) throws MyEntityNotFoundException, MyReflectionException {
        try {
            if (entityClass == null) throw new MyReflectionException(null, "finding generic type or class: ENTITY");
            E entity = null;
            if (primaryKey != null) entity = em.find(entityClass, primaryKey);
            if (entity == null) throw new MyEntityNotFoundException(entityClass, primaryKey);
            return entity;
        } catch (MyReflectionException | MyEntityNotFoundException e) {
            throw e;
        } catch (Exception e) {
            throw new EJBException(errorMsg("finding"), e);
        }
    }

    public E findOrFail(Object primaryKey) throws MyReflectionException {
        try {
            if (entityClass == null) throw new MyReflectionException(null, "finding generic type or class: ENTITY");
            if (primaryKey == null) return null;
            return em.find(entityClass, primaryKey);
        } catch (MyReflectionException e) {
            throw e;
        } catch (Exception e) {
            throw new EJBException(errorMsg("finding"), e);
        }
    }

    public String errorMsg(String operation) {
        return new StringBuilder("ERROR_").append(operation.toUpperCase()).append("_").append(entityClass != null ? entityClass.getSimpleName().toUpperCase() : "ENTITY").toString();
    }

    public Class<E> entityClass() {
        return entityClass;
    }

    public Class<D> dtoClass() {
        return dtoClass;
    }

    protected void initialize(Class<E> entityClass, Class<D> dtoClass) {
        this.entityClass = entityClass;
        this.dtoClass = dtoClass;
    }

    protected abstract void init();

    protected abstract Object primaryKey(D dto);

    protected abstract D map(E entity, D dto);

    protected abstract E map(D dto, E entity);
}
