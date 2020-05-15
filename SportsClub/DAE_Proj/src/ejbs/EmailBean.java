package ejbs;

import dtos.EmailDTO;
import entities.Email;

import javax.annotation.PostConstruct;
import javax.annotation.Resource;
import javax.ejb.EJB;
import javax.ejb.EJBException;
import javax.ejb.Stateless;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Message;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import java.util.Date;
import java.util.List;

@Stateless(name = "EmailEJB")
public class EmailBean extends Bean<Email, EmailDTO> {

    @PersistenceContext
    protected EntityManager em;

    @Resource(name = "java:/jboss/mail/gmail")
    private Session mailSession;

    @EJB
    private UserBean userBean;

    public void send(EmailDTO emailDTO) throws MessagingException {
        if (emailDTO == null) return;
        Message message = new MimeMessage(mailSession);
        try {
            message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(emailDTO.getUser().getEmail(), false));
            message.setSubject(emailDTO.getSubject());
            message.setText(emailDTO.getMessage());
            Date timeStamp = new Date();
            message.setSentDate(timeStamp);
            Transport.send(message);
            create(emailDTO);
        } catch (MessagingException e) {
            throw e;
        } catch (Exception e) {
            throw new EJBException(errorMsg("sending"), e);
        }
    }

    public List<Email> emails(String username) {
        try {
            return (List<Email>) em.createNamedQuery("email.username").setParameter("username", username).getResultList();
        } catch (Exception e) {
            throw new EJBException(errorMsg("retrieving") + "S", e);
        }
    }

    @Override
    public Object primaryKey(EmailDTO dto) {
        return dto.getId();
    }

    @Override
    protected EmailDTO map(Email entity, EmailDTO dto) {
        dto.setId(entity.getId());
        dto.setSubject(entity.getSubject());
        dto.setMessage(entity.getMessage());
        dto.setUser(userBean.toDTO(entity.getUser()));
        return dto;
    }

    @Override
    protected Email map(EmailDTO dto, Email entity) {
        entity.setId(dto.getId());
        entity.setSubject(dto.getSubject());
        entity.setMessage(dto.getMessage());
        entity.setUser(userBean.toEntity(dto.getUser()));
        return entity;
    }

    @PostConstruct
    protected void init() {
        initialize(Email.class, EmailDTO.class);
    }
}
