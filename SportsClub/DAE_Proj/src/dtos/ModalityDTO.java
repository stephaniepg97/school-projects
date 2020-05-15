package dtos;

import java.util.*;
import java.util.stream.Collectors;

public class ModalityDTO implements DTO {

    private Integer id;
    private Set<ProductDTO> products;
    private String name;

    public ModalityDTO() {
        this.products = new LinkedHashSet<>();
    }

    public ModalityDTO(Integer id, String name) {
        this.id = id;
        this.name = name;
        this.products = new LinkedHashSet<>();
    }

    public ModalityDTO(String name) {
        this(null, name);
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setProducts(Collection<ProductDTO> products) {
        this.products = products.stream().collect(Collectors.toSet());
    }

    public Set<ProductDTO> getProducts() {
        return products;
    }

    public void setProducts(Set<ProductDTO> products) {
        this.products = products;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void add(ProductDTO p) {
        this.products.add(p);
    }

    public void remove(ProductDTO p) {
        this.products.remove(p);
    }

    @Override
    public void reset() {
        this.name = null;
        this.id = null;
        this.products.clear();
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ModalityDTO that = (ModalityDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
