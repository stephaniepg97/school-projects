package dtos;

import java.util.LinkedHashSet;
import java.util.Set;

public class CategoryDTO implements DTO {

    private String name;
    private Set<GenericProductDTO> products;

    public CategoryDTO() {
        this.products = new LinkedHashSet<>();
    }

    public CategoryDTO(String name) {
        this.name = name;
        this.products = new LinkedHashSet<>();
    }

    public void add(GenericProductDTO p) {
        this.products.add(p);
    }

    public void remove(GenericProductDTO p) { this.products.remove(p); }

    public String getName() { return name; }

    public void setName(String name) { this.name = name; }

    public Set<GenericProductDTO> getProducts() {
        return products;
    }

    public void setProducts(Set<GenericProductDTO> products) {
        this.products = products;
    }

    @Override
    public void reset() {
        this.name = null;
        this.products.clear();
    }
}
