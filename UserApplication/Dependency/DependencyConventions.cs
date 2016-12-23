using System.Web.Mvc;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ServiceLayer;
using System.Web.Http.Controllers;
using ServiceLayer.GroupManager;

namespace WebApplication.Dependency
{
    public class DependencyConventions : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                .BasedOn<IController>()
                                .LifestyleTransient());

            container.Register(

                        //Component.For<IQueryableUnitOfWork, UnitOfWork>().ImplementedBy<UnitOfWork>(),

                        //Component.For<IProfileRepository, ProfileRepository>().ImplementedBy<ProfileRepository>(),

                        //Component.For<IAddressRepository, AddressRepository>().ImplementedBy<AddressRepository>(),

                        //Component.For<IAddressTypeRepository, AddressTypeRepository>().ImplementedBy<AddressTypeRepository>(),

                        //Component.For<IPhoneTypeRepository, PhoneTypeRepository>().ImplementedBy<PhoneTypeRepository>(),

                        //Component.For<IPhoneRepository, PhoneRepository>().ImplementedBy<PhoneRepository>(),

                        //Component.For<IAnswersRepository, AnswersRepository>().ImplementedBy<AnswersRepository>(),

                        //Component.For<IQuestionsRepository, QuestionsRepository>().ImplementedBy<QuestionsRepository>(),//.LifestyleSingleton(),

                        //Component.For<ISecurityRepository, SecurityRepository>().ImplementedBy<SecurityRepository>(),

                        //Component.For<ISecurityPriceRepository, SecurityPriceRepository>().ImplementedBy<SecurityPriceRepository>(),

                        //Component.For<IDailyPriceRepository, DailyPriceRepository>().ImplementedBy<DailyPriceRepository>(),

                        //Component.For<ICommentsRepository, CommentsRepository>().ImplementedBy<CommentsRepository>(),

                        //Component.For<ITweetRepository, TweetRepository>().ImplementedBy<TweetRepository>(),

                        //Component.For<IUserRepository, UserRepository>().ImplementedBy<UserRepository>(),

                        ////Component.For<ILoggerManager>().ImplementedBy<LoggerManager>(),

                        //Component.For<IRiskManager>().ImplementedBy<RiskManager>(),

                        //Component.For<ISecurityManager>().ImplementedBy<SecurityManager>(),

                        //Component.For<ICommentManager>().ImplementedBy<CommentManager>(),

                        //Component.For<ITweetManager>().ImplementedBy<TweetManager>(),

                        Component.For<IUserManager>().ImplementedBy<UserManager>(),

                        Component.For<IRouteManager>().ImplementedBy<RouteManager>(),

                        Component.For<IGroupManager>().ImplementedBy<GroupManager>(),

                        AllTypes.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient()

                        )
                       .AddFacility<LoggingFacility>(f => f.UseLog4Net());

            //LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            //EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());


        }
        
    }
}