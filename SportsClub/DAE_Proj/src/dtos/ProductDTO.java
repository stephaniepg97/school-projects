package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class ProductDTO extends GenericProductDTO {

    protected ModalityDTO modality;
    protected Set<RegistrationDTO> registrations;

    public ProductDTO(Integer id, CategoryDTO category, ModalityDTO modality, String name, double value, int stock, Integer originalId) {
        super(id, category, name, value, stock, originalId);
        this.modality = modality;
        this.registrations = new LinkedHashSet<>();
    }

    public ProductDTO(CategoryDTO category, ModalityDTO modality, String name, double value, int stock, Integer originalId) {
        this(null, category, modality, name, value, stock, originalId);
    }

    public ProductDTO(CategoryDTO category, ModalityDTO modality, String name, double value, int stock) {
        this(null, category, modality, name, value, stock, null);
    }

    public ProductDTO() {
        this.registrations = new LinkedHashSet<>();
    }

    public void add(RegistrationDTO r) {
        this.registrations.add(r);
    }

    public void remove(RegistrationDTO r) {
        this.registrations.remove(r);
    }

    public void removeRegistration(int id) {
        List<RegistrationDTO> result = this.registrations.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Set<RegistrationDTO> getRegistrations() {
        return registrations;
    }

    public void setRegistrations(Collection<RegistrationDTO> registrations) {
        this.registrations = registrations.stream().collect(Collectors.toSet());
    }

    public ModalityDTO getModality() {
        return modality;
    }

    public void setModality(ModalityDTO modality) {
        this.modality = modality;
    }

    @Override
    public void reset() {
        super.reset();
        this.modality.reset();
        this.registrations.clear();
    }
}
