package dtos;

import java.util.LinkedHashSet;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class RegistrationDTO implements DTO {

    private Integer id;
    private AthleteDTO athlete;
    private Set<ProductDTO> products;

    public RegistrationDTO() {
        this.products = new LinkedHashSet<>();
    }

    public RegistrationDTO(Integer id, AthleteDTO athlete) {
        this.id = id;
        this.athlete = athlete;
        this.products = new LinkedHashSet<>();
    }

    public RegistrationDTO(AthleteDTO athlete) {
        this(null, athlete);
    }

    public void add(ProductDTO p) {
        this.products.add(p);
    }

    public void remove(ProductDTO p) { this.products.remove(p); }

    public void removeProduct(int id) {
        List<ProductDTO> result = this.products.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Set<ProductDTO> getProducts() {
        return products;
    }

    public void setProducts(Set<ProductDTO> products) {
        this.products = products;
    }

    public AthleteDTO getAthlete() {
        return athlete;
    }

    public void setAthlete(AthleteDTO athlete) {
        this.athlete = athlete;
    }

    @Override
    public void reset() {
        this.id = null;
        this.athlete.reset();
        this.products.clear();
    }
}
