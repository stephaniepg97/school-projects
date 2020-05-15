package dtos;

public class GraduationDTO extends ProductDTO {

    public GraduationDTO(Integer id, ModalityDTO modality, String name, double value, int stock, Integer originalId) {
        super(id, new CategoryDTO("Graduation"), modality, name, value, stock, originalId);
    }

    public GraduationDTO(ModalityDTO modality, String name, double value, int stock, Integer originalId) {
        this(null, modality, name, value, stock, originalId);
    }

    public GraduationDTO(ModalityDTO modality, String name, double value, int stock) {
        this(null, modality, name, value, stock, null);
    }

    public GraduationDTO() {
    }
}
