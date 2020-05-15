package dtos;

import java.util.Collection;
import java.util.LinkedHashSet;
import java.util.Objects;
import java.util.Set;
import java.util.stream.Collectors;

public class GenericProductDTO implements DTO {

    private Integer id;
    private CategoryDTO category;
    private String name;
    private double value;
    private int stock;
    private Integer originalId;
    private Set<PaymentDTO> payments;

    public GenericProductDTO() {
    }

    public GenericProductDTO(Integer id, CategoryDTO category, String name, double value, int stock, Integer originalId) {
        this.id = id;
        this.category = category;
        this.name = name;
        this.value = value;
        this.stock = stock;
        this.originalId = originalId;
        this.payments = new LinkedHashSet<>();
    }

    public GenericProductDTO(CategoryDTO category, String name, double value, int stock, Integer originalId) {
        this(null, category, name, value, stock, originalId);
    }

    public GenericProductDTO(CategoryDTO category, String name, double value, int stock) {
        this(null, category, name, value, stock, null);
    }

    public void add(PaymentDTO p) {
        this.payments.add(p);
    }

    public void remove(PaymentDTO p) { this.payments.remove(p); }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setValue(double value) {
        this.value = value;
    }

    public CategoryDTO getCategory() {
        return category;
    }

    public void setCategory(CategoryDTO category) {
        this.category = category;
    }

    public Set<PaymentDTO> getPayments() {
        return payments;
    }

    public void setPayments(Collection<PaymentDTO> payments) {
        this.payments = payments.stream().collect(Collectors.toSet());
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Double getValue() {
        return value;
    }

    public void setValue(Double value) {
        this.value = value;
    }

    public Integer getOriginalId() {
        return originalId;
    }

    public void setOriginalId(Integer originalId) {
        this.originalId = originalId;
    }

    public int getStock() {
        return stock;
    }

    public void setStock(int stock) {
        this.stock = stock;
    }

    @Override
    public void reset() {
        this.category.reset();
        this.payments.clear();;
        this.originalId = this.id = this.stock = -1;
        this.value = -1;
        this.name = null;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        GenericProductDTO that = (GenericProductDTO) o;
        return id == that.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
