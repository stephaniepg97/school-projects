package dtos;

import java.util.Objects;

public class EmailDTO implements DTO {

    private UserDTO user;
    private int id;
    private String subject;
    private String message;

    public EmailDTO() {
    }

    public EmailDTO(int id, UserDTO user, String subject, String message) {
        this.id = id;
        this.user = user;
        this.subject = subject;
        this.message = message;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public UserDTO getUser() {
        return user;
    }

    public void setUser(UserDTO user) {
        this.user = user;
    }

    public String getSubject() {
        return subject;
    }

    public void setSubject(String subject) {
        this.subject = subject;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    @Override
    public void reset() {
        this.user.reset();
        this.id = -1;
        this.subject = this.message = null;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        EmailDTO emailDTO = (EmailDTO) o;
        return id == emailDTO.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
