package entities;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Objects;
import javax.persistence.Entity;

@Entity
@Table(name = "EMAILS")
@NamedQueries({
        @NamedQuery(
                name = "email.all",
                query = "SELECT e FROM Email e"
        ),
        @NamedQuery(
                name = "email.username",
                query = "SELECT e FROM Email e WHERE e.user.username = :username"
        )
})
public class Email implements entities.Entity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private Integer id;

    @NotNull
    @ManyToOne
    private User user;

    @NotNull
    private String subject;

    @NotNull
    private String message;

    @Version
    private int version;

    public Email() {
    }

    public Email(User user, String subject, String message) {
        this.user = user;
        this.subject = subject;
        this.message = message;
    }

    public Email(Integer id, User user, String subject, String message) {
        this(user, subject, message);
        this.id = id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getId() {
        return id;
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
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
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Email email = (Email) o;
        return id == email.id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
